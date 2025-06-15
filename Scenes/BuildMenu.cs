using Godot;
using System;

public partial class BuildMenu : Control
{
    [Export] public PackedScene MachinePrefab;
    [Export] public PackedScene BeltPrefab;

    public static PackedScene SelectedPrefab;
    public static Vector2I CurrentDirection = new Vector2I(1, 0); // Default: Right

    private static readonly Vector2I[] Directions = new Vector2I[]
    {
        new Vector2I(1, 0),  // Right
        new Vector2I(0, 1),  // Down
        new Vector2I(-1, 0), // Left
        new Vector2I(0, -1), // Up
    };

    private int currentDirectionIndex = 0;

    public override void _Ready()
    {
        var directionLabel = GetNode<Label>("DirectionLabel");

        GetNode<Button>("MachineButton").Pressed += () =>
        {
            SelectedPrefab = MachinePrefab;
            GD.Print("✅ Selected: Machine");
        };

        GetNode<Button>("BeltButton").Pressed += () =>
        {
            SelectedPrefab = BeltPrefab;
            GD.Print("✅ Selected: Belt");
        };

        GetNode<Button>("RotateButton").Pressed += () =>
        {
            currentDirectionIndex = (currentDirectionIndex + 1) % Directions.Length;
            CurrentDirection = Directions[currentDirectionIndex];

            string arrow = DirectionToArrow(CurrentDirection);
            directionLabel.Text = arrow;

            GD.Print($"🔄 Belt rotation set to: {CurrentDirection}");
        };

        // Initialize UI with default direction
        directionLabel.Text = DirectionToArrow(CurrentDirection);

        SelectedPrefab = MachinePrefab;
    }

    private string DirectionToArrow(Vector2I dir)
    {
        if (dir == new Vector2I(1, 0)) return "→";
        if (dir == new Vector2I(0, 1)) return "↓";
        if (dir == new Vector2I(-1, 0)) return "←";
        if (dir == new Vector2I(0, -1)) return "↑";
        return "?";
    }
}
