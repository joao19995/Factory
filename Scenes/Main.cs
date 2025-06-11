using System.Reflection.PortableExecutable;
using Godot;

public partial class Main : Node2D
{
    [Export] public PackedScene BeltPrefab;
    [Export] public PackedScene ItemPrefab;
    [Export] public TileMapLayer gridTileMap;
    [Export] public PackedScene MachinePrefab;

    private Vector2 SnapToGrid(Vector2 pos, int gridSize = 16)
    {
        return new Vector2(
            Mathf.Floor(pos.X / gridSize) * gridSize,
            Mathf.Floor(pos.Y / gridSize) * gridSize
        );
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (gridTileMap == null || GridManager.Instance == null || BuildMenu.SelectedPrefab == null)
                return;

            Vector2 mousePos = GetGlobalMousePosition();
            Vector2 localPos = gridTileMap.ToLocal(mousePos);
            Vector2I cell = gridTileMap.LocalToMap(localPos);

            if (!GridManager.Instance.IsCellOccupied(cell))
            {
                var node = BuildMenu.SelectedPrefab.Instantiate<Node2D>();
                node.GlobalPosition = gridTileMap.ToGlobal(gridTileMap.MapToLocal(cell));
                GridManager.Instance.Place(node, cell);
            }
            else
            {
                var occupant = GridManager.Instance.GetNodeAtGridPosition(cell);

                if (BuildMenu.SelectedPrefab == MachinePrefab && occupant is Belt belt)
                {
                    var machine = MachinePrefab.Instantiate<Machine>();
                    machine.GlobalPosition = belt.GlobalPosition;
                    GridManager.Instance.Place(machine, cell);

                    var item = ItemPrefab.Instantiate<Item>();
                    belt.PlaceItem(item);
                }
                else
                {
                    GD.Print("Cell is occupied; can't place selected prefab here.");
                }
            }
        }
    }
}