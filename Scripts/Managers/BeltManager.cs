using Godot;
using System.Collections.Generic;

namespace NovoProjetodejogo.Managers
{
    /// <summary>
    /// Manages belt placement, upgrades, and removal on the factory grid.
    /// </summary>
    public partial class BeltManager : Node
    {
        private TileMapLayer logicLayerMap;
        private TileMapLayer gridTileMap;
        private PackedScene beltPrefab;
        private int beltTileId;

        private readonly Dictionary<Vector2I, Vector2I> beltDirections = [];

        public void Initialize(TileMapLayer logicLayer, TileMapLayer gridLayer, PackedScene prefab, int tileId)
        {
            logicLayerMap = logicLayer;
            gridTileMap = gridLayer;
            beltPrefab = prefab;
            beltTileId = tileId;
        }

        /// <summary>
        /// Creates and configures a new belt instance.
        /// </summary>
        private Belt CreateBelt(Vector2I cell, Vector2I direction)
        {
            var belt = beltPrefab.Instantiate<Belt>();
            belt.Initialize(cell, direction, logicLayerMap);
            belt.Position = gridTileMap.MapToLocal(cell);
            return belt;
        }

        /// <summary>
        /// Places a belt at the specified cell and direction if possible.
        /// </summary>
        public bool PlaceBelt(Vector2I cell, Vector2I direction)
        {
            if (GridManager.Instance.HasStructureAt(cell))
            {
                GD.Print($"[BeltManager] Cell {cell} already occupied!");
                return false;
            }

            SetBeltDirection(cell, direction); // Register direction BEFORE adding to scene
            var belt = CreateBelt(cell, direction);
            gridTileMap.AddChild(belt);
            GridManager.Instance.RegisterStructure(cell, belt);
            logicLayerMap.SetCell(cell, beltTileId);
            return true;
        }

        private void SetBeltDirection(Vector2I cell, Vector2I direction)
        {
            beltDirections[cell] = direction;
        }

        public Vector2I? GetBeltDirection(Vector2I cell)
        {
            if (beltDirections.TryGetValue(cell, out var dir))
                return dir;
            return null;
        }
    }
}
