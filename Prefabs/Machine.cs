using Godot;
using System;

public partial class Machine : Node2D
{
    public void ReceiveItem(Item item)
    {
        GD.Print($"Machine received item {item.Name}");
        // TODO: Process item
        item.QueueFree(); // example: consume item
    }
}

