using Godot; // Godot engine namespace
using System.Collections.Generic; // For using Dictionary

public partial class GridManager : Node2D // GridManager inherits from Node2D for 2D grid logic
{
    public static GridManager Instance; // Singleton instance for global access

    private Dictionary<Vector2I, Node2D> gridContents = new(); // Stores structures at grid positions

    public override void _Ready()
    {
        Instance = this; // Set singleton instance when node is ready
    }

    public void RegisterStructure(Vector2I position, Node2D structure)
    {
        gridContents[position] = structure; // Register or update structure at given grid position
    }

    public bool HasStructureAt(Vector2I position)
    {
        return gridContents.ContainsKey(position); // Check if a structure exists at the given position
    }

    public Node2D GetStructureAt(Vector2I position)
    {
        return gridContents.TryGetValue(position, out var structure) ? structure : null; // Get structure or null if not found
    }
}

