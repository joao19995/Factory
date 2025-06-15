using Godot;
namespace NovoProjetodejogo.Scripts.Items
{
    public partial class Item : CharacterBody2D
    {
        public float Speed = 1f;  // Movement speed in pixels per seconds
        public Vector2 Direction = Vector2.Right;  // Movement direction, to be set by Belt

            public ItemType Type { get; private set; }

    // Constructor or init method
    public void Initialize(ItemType type)
    {
        Type = type;
        // Update visuals based on type, e.g.:
        UpdateSpriteForType(type);
    }

    private void UpdateSpriteForType(ItemType type)
    {
        // Change sprite or animation depending on type
    }
    }
}

