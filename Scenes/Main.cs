using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using Godot;
using NovoProjetodejogo.Scripts.Items;
using NovoProjetodejogo.Scenes;
using NovoProjetodejogo.Managers;

public partial class Main : Node2D
{
    [Export] public PackedScene BeltPrefab; // Prefab for belt placement
    [Export] public PackedScene ItemPrefab; // Prefab for items to be placed on belts
    [Export] public PackedScene MachinePrefab; // Prefab for machines
    [Export] public TileMapLayer gridTileMap;  // Visual map
    [Export] public TileMapLayer logicLayerMap; // Logic map
    [Export] public Node2D ItemLayer;
    [Export] public int beltTileId = 0;

    private BeltManager beltManager;
    private MachineManager machineManager;

    public override void _Ready()
    {
        beltManager = GetNodeOrNull<NovoProjetodejogo.Managers.BeltManager>("BeltManager");
        beltManager.Initialize(logicLayerMap, gridTileMap, BeltPrefab, beltTileId);

        machineManager = GetNodeOrNull<MachineManager>("MachineManager");
        machineManager.Initialize(gridTileMap, MachinePrefab, GridManager.Instance);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (gridTileMap == null || GridManager.Instance == null || BuildMenu.SelectedPrefab == null)
                return;

            Vector2I cell = gridTileMap.LocalToMap(gridTileMap.ToLocal(GetGlobalMousePosition()));

            GD.Print($"Mouse clicked at cell: {cell}");

            if (BuildMenu.SelectedPrefab == BeltPrefab)
            {
                PlaceBeltAt(cell, BuildMenu.CurrentDirection);
            }
            else if (BuildMenu.SelectedPrefab == MachinePrefab)
            {
                PlaceMachineAt(cell);
            }
            else if (BuildMenu.SelectedPrefab == ItemPrefab)
            {
                PlaceItemAt(cell);
            }
        }

        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.T)
        {
            Vector2I testPos = new(10, 10);
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
        if (!beltManager.PlaceBelt(cell, direction))
        {
            GD.Print($"Cell {cell} already occupied or placement failed!");
        }
        else
        {
            GD.Print($"Placed belt at {cell} with direction {direction}");
        }
    }

    public void PlaceMachineAt(Vector2I cell)
    {
        machineManager.PlaceMachineAt(cell);
    }

    public void PlaceItemAt(Vector2I cell)
    {
        if (ItemPrefab == null)
        {
            GD.PrintErr("[Main] ItemPrefab is not set!");
            return;
        }
        var structure = GridManager.Instance.GetStructureAt(cell);
        if (structure is Belt belt)
        {
            var item = ItemPrefab.Instantiate<Item>();
            item.Initialize(ItemType.Iron); // Or use a parameter for item type if needed
            belt.PlaceItem(item);
            GD.Print($"[Main] Placed item on belt at {cell}");
        }
        else
        {
            GD.PrintErr($"[Main] No belt at {cell} to place item on!");
        }
    }
}
