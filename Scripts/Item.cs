using Godot;

public partial class Item : Node2D
{
    [Export]
    public float Speed = 100f;

    private Vector2 targetPosition;
    private bool moving = false;

    public bool IsMoving => moving;

    public void MoveTo(Vector2 target)
    {
        targetPosition = target;
        moving = true;
    }

    public override void _Process(double delta)
    {
        if (!moving) return;

        Vector2 direction = (targetPosition - Position).Normalized();
        float distanceToMove = Speed * (float)delta;

        if (Position.DistanceTo(targetPosition) <= distanceToMove)
        {
            Position = targetPosition;
            moving = false;
        }
        else
        {
            Position += direction * distanceToMove;
        }
    }
}

