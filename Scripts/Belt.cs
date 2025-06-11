using Godot;

public partial class Belt : Node2D
{
    private Item currentItem = null;
    [Export]
    public Vector2I Direction = new Vector2I(1, 0); // Right by default

    public Vector2I GridPosition; // You can assign this when placing

    public void PlaceItem(Item item)
    {
        currentItem = item;
        AddChild(item);
        item.Position = Vector2.Zero; // local to this belt
        MoveItemToNextPosition();
    }

    private void MoveItemToNextPosition()
    {
        if (currentItem == null) return;

        Vector2 nextPos = new Vector2(Direction.X * 16, Direction.Y * 16); // 16x16 tile size
        currentItem.MoveTo(nextPos);
    }

public override void _Process(double delta)
{
    if (currentItem == null) return;

    if (!currentItem.IsMoving)
    {
        Vector2I nextGridPos = GridPosition + Direction;
        Belt nextBelt = GridManager.Instance.GetBeltAtGridPosition(nextGridPos);

        if (nextBelt != null && nextBelt.currentItem == null)
        {
            // Transfer to next belt
            currentItem.Position = Vector2.Zero;
            RemoveChild(currentItem);

            nextBelt.PlaceItem(currentItem);
            currentItem = null;
        }
        else
        {
            // No next belt or it's occupied → item exits belt here
            OnItemExit(currentItem);
            RemoveChild(currentItem);
            currentItem.QueueFree();  // or handle differently
            currentItem = null;
        }
    }
}
protected void OnItemExit(Item item)
{
    Vector2I exitPos = GridPosition + Direction;
    var node = GridManager.Instance.GetNodeAtGridPosition(exitPos);

    if (node is Machine machine)
    {
        GD.Print($"Item sent to machine at {exitPos}");
        machine.ReceiveItem(item);
    }
    else
    {
        GD.Print("No machine to receive item; item destroyed.");
        item.QueueFree();
    }
}



}
