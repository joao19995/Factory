// ========================================
// Project: Industrial Genesis (Godot 4.4)
// Category: Managers
// File: MachineManager.cs
// Role: MachineManager – Handles machine placement, direction registration, and lookup
// Priority: High
// Dependencies: GridManager, Machine, TileMap
// Architecture: Component-based, modular, signal-driven
// Key Behaviors:
// 1. Place machines on the grid
// 2. Register and lookup machine input directions
// 3. Provide rotation utility for machine facing
// Guidelines:
// - Use clean, idiomatic Godot C#
// - Use signals for communication
// - Avoid logic inside _Process unless it's time-sensitive
// - Comment all key methods and purpose-driven logic
// ========================================

using Godot;
using System.Collections.Generic;

/// <summary>
/// Manages machine placement, input direction registration, and direction lookup on the grid.
/// </summary>
public partial class MachineManager : Node
{
    private readonly Dictionary<Vector2I, Vector2I> machineInputDirections = [];
    private TileMapLayer gridTileMap;
    private PackedScene machinePrefab;
    private GridManager gridManager;

    /// <summary>
    /// Initializes the MachineManager with required dependencies.
    /// </summary>
    public void Initialize(TileMapLayer tileMap, PackedScene machinePrefab, GridManager gridManager)
    {
        this.gridTileMap = tileMap;
        this.machinePrefab = machinePrefab;
        this.gridManager = gridManager;
    }

    /// <summary>
    /// Places a machine at the specified cell, sets its direction, and registers it with the grid.
    /// </summary>
    public void PlaceMachineAt(Vector2I cell)
    {
        if (gridManager.HasStructureAt(cell))
        {
            GD.Print($"Cell {cell} already occupied!");
            return;
        }

        var machine = machinePrefab.Instantiate<Machine>();
        machine.Initialize(cell);
        machine.Position = gridTileMap.MapToLocal(cell);

        var dir = NovoProjetodejogo.Scenes.BuildMenu.CurrentDirection;
        SetMachineInputDirection(cell, -dir); // Input comes from behind
        machine.Rotation = GetRotationFromDirection(dir);

        gridTileMap.AddChild(machine);
        gridManager.RegisterStructure(cell, machine);
        GD.Print($"🛠️ Placed machine at {cell} facing {dir}");
    }

    /// <summary>
    /// Registers the input direction for a machine at a given cell.
    /// </summary>
    public void SetMachineInputDirection(Vector2I cell, Vector2I direction)
    {
        machineInputDirections[cell] = direction;
    }

    /// <summary>
    /// Gets the input direction for a machine at a given cell, if registered.
    /// </summary>
    public Vector2I? GetMachineInputDirection(Vector2I cell)
    {
        if (machineInputDirections.TryGetValue(cell, out var dir))
            return dir;
        return null;
    }

    /// <summary>
    /// Returns the rotation (in radians) for a given direction vector.
    /// </summary>
    public static float GetRotationFromDirection(Vector2I dir)
    {
        if (dir == Vector2I.Right) return 0f;
        if (dir == Vector2I.Down) return Mathf.Pi / 2f;
        if (dir == Vector2I.Left) return Mathf.Pi;
        if (dir == Vector2I.Up) return -Mathf.Pi / 2f;
        return 0f;
    }

    /// <summary>
    /// Gets the machine instance at a given cell, or null if none exists.
    /// </summary>
    public Machine GetMachineAt(Vector2I cell)
    {
        if (gridManager.HasStructureAt(cell))
        {
            var node = gridManager.GetStructureAt(cell);
            if (node is Machine machine)
                return machine;
        }
        return null;
    }
}
