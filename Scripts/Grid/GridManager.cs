using Godot;
using System.Collections.Generic;

public partial class GridManager : Node2D
{
    public static GridManager Instance;

    private Dictionary<Vector2I, Node2D> gridContents = new();

    private TileMapLayer gridTileMap;

    public override void _Ready()
    {
        Instance = this;

        // Replace with your actual node path if named differently
        gridTileMap = GetParent().GetNode<TileMapLayer>("GridTileMap");
    }
    public Node2D GetNodeAtGridPosition(Vector2I gridPos)
    {
        if (gridContents.TryGetValue(gridPos, out var node))
            return node;
        return null;
    }

    public Vector2I WorldToGrid(Vector2 worldPos)
    {
        // Convert world → local → grid cell
        Vector2 local = gridTileMap.ToLocal(worldPos);
        Vector2I cell = gridTileMap.LocalToMap(local);
        return cell;
    }

    public Vector2 GridToWorld(Vector2I gridPos)
    {
        // Convert grid cell → local → world
        Vector2 local = gridTileMap.MapToLocal(gridPos);
        Vector2 world = gridTileMap.ToGlobal(local);
        return world;
    }

    public bool IsCellOccupied(Vector2I gridPos)
    {
        return gridContents.ContainsKey(gridPos);
    }

public void Place(Node2D obj, Vector2I gridPos)
{
    if (IsCellOccupied(gridPos))
    {
        GD.Print($"❌ Cell {gridPos} is already occupied!");
        return;
    }

    gridContents[gridPos] = obj;
    obj.Position = GridToWorld(gridPos);
    AddChild(obj);

    if (obj is Belt belt)
        belt.GridPosition = gridPos;
}

    public void Remove(Vector2I gridPos)
    {
        if (gridContents.TryGetValue(gridPos, out var obj))
        {
            obj.QueueFree();
            gridContents.Remove(gridPos);
        }
    }
    public Node2D GetChildAtGridPos(Vector2I gridPos)
    {
        if (gridContents.TryGetValue(gridPos, out var node))
            return node;
        return null;
    }
    public Belt GetBeltAtGridPosition(Vector2I gridPos)
    {
        if (GridManager.Instance.IsCellOccupied(gridPos))
        {
            var node = GridManager.Instance.GetChildAtGridPos(gridPos);
            if (node is Belt belt)
                return belt;
        }
        return null;
    }

}
