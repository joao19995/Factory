using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using Godot;
using NovoProjetodejogo.Scripts.Items;

public partial class Main : Node2D
{
    [Export] public PackedScene BeltPrefab; // Prefab for belt placement
    [Export] public PackedScene ItemPrefab; // Prefab for items to be placed on belts
    [Export] public PackedScene MachinePrefab; // Prefab for machines
    [Export] public TileMapLayer gridTileMap;  // Visual map
    [Export] public TileMapLayer logicLayerMap; // Logic map
    [Export]
    public Node2D ItemLayer;

    // You need to export or assign the belt tile id in the logicLayer tileset
    [Export] public int beltTileId = 0;
    private Dictionary<Vector2I, Vector2I> beltDirections = new();
    private Dictionary<Vector2I, Vector2I> machineInputDirections = new();

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (gridTileMap == null || GridManager.Instance == null || BuildMenu.SelectedPrefab == null)
                return;

            Vector2 mousePos = GetGlobalMousePosition();
            Vector2 localPos = gridTileMap.ToLocal(mousePos);
            Vector2I cell = gridTileMap.LocalToMap(localPos);

            GD.Print($"Mouse clicked at cell: {cell}");

            if (BuildMenu.SelectedPrefab == BeltPrefab)
            {
                PlaceBeltAt(cell, BuildMenu.CurrentDirection);
            }
            else if (BuildMenu.SelectedPrefab == MachinePrefab)
            {
                PlaceMachineAt(cell);
            }

        }

        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.T)
        {
            Vector2I testPos = new Vector2I(10, 10);
            if (GridManager.Instance.GetStructureAt(testPos) is Belt belt)
            {
                if (ItemPrefab != null)
                {
                    var item = ItemPrefab.Instantiate<Item>();
                    item.Initialize(ItemType.Iron);
                    belt.PlaceItem(item);
                    GD.Print("Placed item on belt at ", testPos);
                }
            }
        }
    }

    public void PlaceBeltAt(Vector2I cell, Vector2I direction)
    {
        if (GridManager.Instance.HasStructureAt(cell))
        {
            GD.Print($"Cell {cell} already occupied!");
            return;
        }

        if (beltTileId < 0)
        {
            GD.PrintErr("beltTileId is not set! Please assign a valid tile ID.");
            return;
        }
        SetBeltDirection(cell, direction);

        // Instantiate belt visual node
        var belt = BeltPrefab.Instantiate<Belt>();
        belt.LogicLayerMap = logicLayerMap;  // assign first
        belt.Position = gridTileMap.MapToLocal(cell);
        belt.GridPosition = cell;
        belt.Direction = direction;
        gridTileMap.AddChild(belt);

        GridManager.Instance.RegisterStructure(cell, belt);

        // Set belt tile in logic layer map
        GD.Print($"Setting tile at {cell} with ID {beltTileId}");

        logicLayerMap.SetCell(cell, beltTileId);
        var mapSize = logicLayerMap.GetUsedRect();
        GD.Print($"Tilemap used rect: {mapSize}");
        GD.Print($"Trying to place at cell: {cell}");

        // Store direction in dictionary instead of tile data

        GD.Print($"Placed belt at {cell} with direction {direction}");
    }
    public void PlaceMachineAt(Vector2I cell)
    {
        if (GridManager.Instance.HasStructureAt(cell))
        {
            GD.Print($"Cell {cell} already occupied!");
            return;
        }

        var machine = MachinePrefab.Instantiate<Machine>();
        machine.Initialize(cell);
        machine.Position = gridTileMap.MapToLocal(cell);

        var dir = BuildMenu.CurrentDirection;
        SetMachineInputDirection(cell, -dir); // Input comes from behind
        machine.Rotation = GetRotationFromDirection(dir);

        gridTileMap.AddChild(machine);
        GridManager.Instance.RegisterStructure(cell, machine);
        GD.Print($"🛠️ Placed machine at {cell} facing {dir}");
    }
    public Vector2I? GetBeltDirection(Vector2I cell)
    {
        if (beltDirections.TryGetValue(cell, out var dir))
            return dir;
        return null;
    }

    public void SetBeltDirection(Vector2I cell, Vector2I direction)
    {
        beltDirections[cell] = direction;
    }
    public void SetMachineInputDirection(Vector2I cell, Vector2I direction)
    {
        machineInputDirections[cell] = direction;
    }

    public Vector2I? GetMachineInputDirection(Vector2I cell)
    {
        if (machineInputDirections.TryGetValue(cell, out var dir))
            return dir;
        return null;
    }
    public float GetRotationFromDirection(Vector2I dir)
    {
        if (dir == Vector2I.Right) return 0f;
        if (dir == Vector2I.Down) return Mathf.Pi / 2f;
        if (dir == Vector2I.Left) return Mathf.Pi;
        if (dir == Vector2I.Up) return -Mathf.Pi / 2f;
        return 0f;
    }
}
