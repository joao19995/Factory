using Godot;
using System;

public partial class BuildMenu : Control
{
    [Export]
    public PackedScene MachinePrefab;

    [Export]
    public PackedScene BeltPrefab;

    public static PackedScene SelectedPrefab;

    public override void _Ready()
    {
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

        // Default selected
        SelectedPrefab = MachinePrefab;
    }
}
