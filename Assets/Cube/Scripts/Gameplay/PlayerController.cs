using System;

using UnityEngine;

using Cube.Level;

namespace Cube.Gameplay
{
    public sealed class PlayerController : MonoBehaviour
    {
        [NonSerialized]
        public Vector2Int Direction = Vector2Int.zero;

        [SerializeField]
        private PlayerView _view = null;

        private LevelData _level = null;
        private Vector2Int _position = Vector2Int.zero;

        public void Init(LevelData level)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            _level = level;

            Vector2Int startPosition;
            if (!level.TryGetTilePosition(Tile.Start, out startPosition))
            {
                Debug.LogWarning("Start position not found.");
            }
            _position = startPosition;
            _view.Init(startPosition);
        }

        private void Update()
        {
            if (_view.Moving)
            {
                // Move animation must finish before position can be updated again.
                return;
            }

            Vector2Int direction = Direction;
            if (direction != Vector2Int.zero)
            {
                TryMove(direction);
            }
        }

        private void TryMove(Vector2Int direction)
        {
            Vector2Int position = _position + direction;
            LevelData level = _level;
            if (level.IsValidPosition(position))
            {
                Tile tile = level.Tiles[position.y, position.x];
                if ((tile != Tile.Void) && (tile != Tile.Wall))
                {
                    _position = position;
                    _view.StartMove(direction);
                }
            }
        }
    }
}
