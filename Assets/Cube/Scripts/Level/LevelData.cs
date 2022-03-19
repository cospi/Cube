using System;

using UnityEngine;

namespace Cube.Level
{
    public sealed class LevelData
    {
        public event Action OnTileChanged = delegate { };
        public event Action OnSizeChanged = delegate { };

        public TileData[,] Tiles = null;

        public LevelData(Vector2Int size)
        {
            Tiles = new TileData[size.y, size.x];
        }

        public void SetSize(Vector2Int size)
        {
            if ((size.x <= 0) || (size.y <= 0))
            {
                throw new ArgumentException($"{nameof(size)} must be positive.", nameof(size));
            }

            Vector2Int previousSize = GetSize();
            if (size == previousSize)
            {
                return;
            }

            TileData[,] previousTiles = Tiles;
            TileData[,] newTiles = new TileData[size.y, size.x];
            Vector2Int copySize = Vector2Int.Min(size, previousSize);
            for (int y = 0; y < copySize.y; ++y)
            {
                for (int x = 0; x < copySize.x; ++x)
                {
                    newTiles[y, x] = previousTiles[y, x];
                }
            }
            Tiles = newTiles;
            OnSizeChanged();
        }

        public void SetTile(Vector2Int position, TileData tile)
        {
            if (!IsValidPosition(position))
            {
                Debug.LogError($"Invalid position {position}.");
                return;
            }

            TileData existingTile = Tiles[position.y, position.x];
            if (!tile.Equals(existingTile))
            {
                Tiles[position.y, position.x] = tile;
                OnTileChanged();
            }
        }

        public Vector2Int GetSize()
        {
            TileData[,] tiles = Tiles;
            return new Vector2Int(tiles.GetLength(1), tiles.GetLength(0));
        }

        public bool IsValidPosition(Vector2Int position)
        {
            if ((position.x < 0) || (position.y < 0))
            {
                return false;
            }

            Vector2Int size = GetSize();
            return (position.x < size.x) && (position.y < size.y);
        }
    }
}
