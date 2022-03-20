using System;
using System.Linq;

using UnityEngine;

namespace Cube.Level
{
    public sealed class LevelData
    {
        public event Action OnTileChanged = delegate { };
        public event Action OnSizeChanged = delegate { };

        public Tile[,] Tiles { get; private set; } = null;

        public LevelData(Vector2Int size)
        {
            if ((size.x <= 0) || (size.y <= 0))
            {
                throw new ArgumentException($"{nameof(size)} must be positive.", nameof(size));
            }

            Tiles = new Tile[size.y, size.x];
        }

        public LevelData(Tile[,] tiles)
        {
            if (tiles == null)
            {
                throw new ArgumentNullException(nameof(tiles));
            }

            Tiles = tiles;
        }

        public void Copy(LevelData level)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            Vector2Int previousSize = GetSize();
            Tiles = level.Tiles;
            if (GetSize() != previousSize)
            {
                OnSizeChanged();
            }
            else
            {
                OnTileChanged();
            }
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

            Tile[,] previousTiles = Tiles;
            Tile[,] newTiles = new Tile[size.y, size.x];
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

        public void SetTile(Vector2Int position, Tile tile)
        {
            if (!IsValidPosition(position))
            {
                Debug.LogError($"Invalid position {position}.");
                return;
            }

            Tile existingTile = Tiles[position.y, position.x];
            if (!tile.Equals(existingTile))
            {
                Tiles[position.y, position.x] = tile;
                // Force only one start/goal.
                if ((tile == Tile.Start) || (tile == Tile.Goal))
                {
                    if (TryGetTilePosition(tile, out Vector2Int existingStartOrGoalPosition, position))
                    {
                        Tiles[existingStartOrGoalPosition.y, existingStartOrGoalPosition.x] = Tile.Floor;
                    }
                }
                OnTileChanged();
            }
        }

        public Vector2Int GetSize()
        {
            Tile[,] tiles = Tiles;
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

        // TODO: Save start and goal position instead of looking them up by iterating over tiles.
        public bool TryGetTilePosition(Tile tile, out Vector2Int position, params Vector2Int[] ignorePositions)
        {
            Tile[,] tiles = Tiles;
            Vector2Int size = GetSize();
            Vector2Int currentPosition = default(Vector2Int);
            for (currentPosition.y = 0; currentPosition.y < size.y; ++currentPosition.y)
            {
                for (currentPosition.x = 0; currentPosition.x < size.x; ++currentPosition.x)
                {
                    if (tiles[currentPosition.y, currentPosition.x] == tile)
                    {
                        if ((ignorePositions == null) || !ignorePositions.Contains(currentPosition))
                        {
                            position = currentPosition;
                            return true;
                        }
                    }
                }
            }
            position = default(Vector2Int);
            return false;
        }
    }
}
