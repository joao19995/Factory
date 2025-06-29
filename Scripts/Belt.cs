using Godot;
using NovoProjetodejogo.Managers;
using NovoProjetodejogo.Scripts.Items;

public partial class Belt : Node2D
{
    private Item currentItem = null;

    [Export] public Vector2I Direction = new(1, 0);
    [Export] public float Speed = 30f; // units per second (pixels/sec)
    public Vector2I GridPosition;

    private float itemProgress = 0f; // movement along belt in pixels (0 to 16)
    public TileMapLayer LogicLayerMap;

    // Reference to your ItemLayer container node, set in _Ready()
    private Node2D itemLayer;

    public override void _Ready()
    {
        if (LogicLayerMap == null)
        {
            GD.PrintErr("[Belt] LogicLayerMap is null in _Ready().");
            return;
        }

        var main = GetTree().Root.GetNodeOrNull<Main>("Main");
        if (main == null)
        {
            GD.PrintErr("[Belt] Could not find Main node.");
            return;
        }

        var beltManager = main.GetNode<BeltManager>("BeltManager");
        if (beltManager == null)
        {
            GD.PrintErr("[Belt] Could not find BeltManager node as a child of Main.");
            return;
        }
        
        var dir = beltManager.GetBeltDirection(GridPosition);
        if (dir != null)
        {
            Direction = dir.Value;
        }

        // Get the ItemLayer from Main
        itemLayer = main.GetNode<Node2D>("ItemLayer");
        if (itemLayer == null)
            GD.PrintErr("[Belt] ItemLayer node not found in Main.");

    }

    public void Initialize(Vector2I gridPos, Vector2I direction, TileMapLayer logicLayer)
    {
        GridPosition = gridPos;
        Direction = direction;
        LogicLayerMap = logicLayer;
    }

    public void PlaceItem(Item item)
    {
        GD.Print($"[Belt] PlaceItem called at {GridPosition} with item {item}");
        // If there is already an item on this belt, do nothing
        if (currentItem != null)
        {
            GD.Print($"[Belt] Already has item at {GridPosition}, cannot accept new item.");
            return;
        }

        // Set the current item to the new item
        currentItem = item;

        // If the item's parent is not the itemLayer, reparent it for correct scene organization
        if (item.GetParent() != itemLayer)
        {
            item.GetParent()?.RemoveChild(item); // Remove from previous parent if any
            itemLayer.AddChild(item); // Add to itemLayer
            GD.Print($"[Belt] Item parent set to ItemLayer at {GridPosition}");
        }

        // Set the item's direction to match the belt's direction
        item.Direction = new Vector2(Direction.X, Direction.Y).Normalized();

        // Reset progress to 0 (start at belt origin)
        itemProgress = 0f;
    }
    public override void _Process(double delta)
    {
        // If there is no item on this belt, do nothing
        if (currentItem == null)
            return;

        // Calculate the next cell in the direction of the belt
        Vector2I nextCell = GridPosition + Direction;
        // Get the structure (if any) at the next cell
        var nextStructure = GridManager.Instance.GetStructureAt(nextCell);
        // Try to cast the structure to a Belt
        Belt nextBelt = nextStructure as Belt;

        // If there is a next belt and it can accept an item
        if (nextBelt != null && nextBelt.CanAcceptItem())
        {
            // Move the item forward along the belt
            itemProgress += Speed * (float)delta;

            // If the item has reached the end of the belt
            if (itemProgress >= 16f)
            {
                // Prepare to transfer the item to the next belt
                var itemToSend = currentItem;
                currentItem = null; // Remove item from this belt
                itemProgress = 0f; // Reset progress

                // Remove the item from the itemLayer if it is still parented there
                if (itemToSend.GetParent() == itemLayer)
                    itemLayer.RemoveChild(itemToSend);

                // Place the item on the next belt
                nextBelt.PlaceItem(itemToSend);
                GD.Print($"[Belt] Transferred item from {GridPosition} to {nextCell}");
            }
            else
            {
                // Update the item's position smoothly along the belt
                currentItem.GlobalPosition = this.GlobalPosition + new Vector2(Direction.X, Direction.Y) * itemProgress;
                GD.Print($"[Belt] Moving item at {GridPosition}: progress={itemProgress}, pos={currentItem.GlobalPosition}");
            }
        }
        else
        {
            // Always update the item's position, even if not moving
            itemProgress = Mathf.Max(itemProgress, 0f); // Clamp to at least 0
            currentItem.GlobalPosition = this.GlobalPosition + new Vector2(Direction.X, Direction.Y) * itemProgress;
            currentItem.Visible = true;
            currentItem.QueueRedraw();
        }
    }

    public bool CanAcceptItem()
    {
        return currentItem == null;
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    // Make TakeItem public so it can be called from Machine
    public Item TakeItem()
    {
        if (currentItem == null)
            return null;

        var item = currentItem;
        currentItem = null;

        if (item.GetParent() == itemLayer)
            itemLayer.RemoveChild(item);

        return item;
    }
}
