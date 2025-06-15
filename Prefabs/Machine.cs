using Godot;
using NovoProjetodejogo.Scripts.Items;

public partial class Machine : Node2D
{
    public Vector2I GridPosition; // Grid cell the machine occupies
    public Vector2I InputDirection = Vector2I.Left; // Default: input comes from left
    public Vector2I OutputDirection => -InputDirection;

    private Item currentItem = null;

    private float processingTime = 10.5f; // Time in seconds to process an item (configurable)
    private float processingTimer = 0f;
    private bool isProcessing = false;

    // Reference to ItemLayer node, set at _Ready()
    private Node2D itemLayer;

    public void Initialize(Vector2I gridPos)
    {
        GridPosition = gridPos;
    }

    public override void _Ready()
    {
        var main = GetTree().Root.GetNodeOrNull<Main>("Main");
        if (main != null)
        {
            var inputDir = main.GetMachineInputDirection(GridPosition);
            if (inputDir != null)
            {
                InputDirection = inputDir.Value;
                GD.Print($"[Machine] Input direction for {GridPosition}: {InputDirection}");
            }
            else
            {
                GD.PrintErr($"[Machine] No input direction found for {GridPosition}");
            }

            // Get the ItemLayer node reference from Main
            itemLayer = main.GetNode<Node2D>("ItemLayer");
            if (itemLayer == null)
                GD.PrintErr("[Machine] ItemLayer node not found in Main");
        }
    }

    public override void _Process(double delta)
    {
        if (currentItem == null)
        {
            // Try to pull from input
            Vector2I inputCell = GridPosition + InputDirection;

            if (GridManager.Instance.GetStructureAt(inputCell) is Belt belt)
            {
                if (belt.HasItem())
                {
                    Item item = belt.TakeItem();
                    if (item != null)
                    {
                        currentItem = item;

                        // Add item as child of ItemLayer for correct layering
                        if (item.GetParent() != itemLayer)
                        {
                            if (item.GetParent() != null)
                                item.GetParent().RemoveChild(item);
                            itemLayer.AddChild(item);
                        }

                        // Here: set the item's position to the machine's position in world coordinates
                        item.GlobalPosition = this.GlobalPosition; // sets the item exactly on the machine's position in the world
                        GD.Print($"Machine global position: {GlobalPosition}");

                        GD.Print($"✅ Machine at {GridPosition} pulled item from {inputCell}");

                        // Start processing timer
                        isProcessing = true;
                        processingTimer = 0f;
                    }
                }
            }
        }
        else
        {
            // If still processing, increment timer and return
            if (isProcessing)
            {
                processingTimer += (float)delta;
                if (processingTimer < processingTime)
                    return;

                // Processing done
                isProcessing = false;
            }

            // Try to push to output
            Vector2I outputCell = GridPosition + OutputDirection;

            if (GridManager.Instance.GetStructureAt(outputCell) is Belt belt)
            {
                if (belt.CanAcceptItem())
                {
                    if (currentItem != null)
                    {
                        // Get the output belt's direction (Vector2I)
                        var beltDir = belt.Direction;
                        currentItem.Direction = new Vector2(beltDir.X, beltDir.Y).Normalized();
                    }
                    belt.PlaceItem(currentItem);

                    // Remove from ItemLayer if still parented there
                    if (currentItem != null && currentItem.GetParent() == itemLayer)
                        itemLayer.RemoveChild(currentItem);

                    GD.Print($"✅ Machine at {GridPosition} pushed item to {outputCell} with direction {OutputDirection}");
                    currentItem = null;
                }
                else
                {
                    GD.Print($"⚠️ Belt at {outputCell} is full, machine at {GridPosition} can't push item");
                }
            }
            else
            {
                GD.Print($"⚠️ No belt at output {outputCell}, machine at {GridPosition} can't push item");
            }
        }
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public Item TakeItem()
    {
        if (currentItem == null)
            return null;

        var item = currentItem;
        currentItem = null;

        // Remove from parent node if needed
        if (item.GetParent() == itemLayer)
            itemLayer.RemoveChild(item);

        return item;
    }
}
