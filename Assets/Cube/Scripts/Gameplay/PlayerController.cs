using System;

using UnityEngine;

using Cube.Level;

namespace Cube.Gameplay
{
    public sealed class PlayerController : MonoBehaviour
    {
        private enum MoveResult
        {
            None,
            LevelFailed,
            LevelCompleted,
            BlockedByWall,
            Moved
        }

        [NonSerialized]
        public Vector2Int Direction = Vector2Int.zero;

        [SerializeField]
        private PlayerView _view = null;

        private LevelData _level = null;
        private Action<bool> _onLevelEnded = null;
        private Vector2Int _position = Vector2Int.zero;
        private MoveResult _moveResult = MoveResult.None;

        public void Init(LevelData level, Action<bool> onLevelEnded)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            _level = level;
            _onLevelEnded = onLevelEnded;

            Vector2Int startPosition;
            if (!level.TryGetTilePosition(Tile.Start, out startPosition))
            {
                Debug.LogWarning("Start position not found.");
            }
            _position = startPosition;
            _moveResult = MoveResult.None;

            _view.Init(startPosition);
        }

        public void Fini()
        {
            Direction.Set(0, 0);
            _view.Fini();
        }

        private void Update()
        {
            if (_view.Moving)
            {
                // Move animation must finish before position can be updated again.
                return;
            }

            MoveResult moveResult = _moveResult;
            if ((moveResult == MoveResult.LevelFailed) || (moveResult == MoveResult.LevelCompleted))
            {
                _onLevelEnded?.Invoke(moveResult == MoveResult.LevelCompleted);
                _moveResult = MoveResult.None;
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
            MoveResult moveResult = GetMoveResult(position);
            if (moveResult != MoveResult.BlockedByWall)
            {
                _position = position;
                _view.StartMove(direction);
            }
            _moveResult = moveResult;
        }

        private MoveResult GetMoveResult(Vector2Int position)
        {
            LevelData level = _level;
            if (!level.IsValidPosition(position))
            {
                return MoveResult.LevelFailed;
            }
            switch (level.Tiles[position.y, position.x])
            {
                case Tile.Void:
                    return MoveResult.LevelFailed;
                case Tile.Wall:
                    return MoveResult.BlockedByWall;
                case Tile.Goal:
                    return MoveResult.LevelCompleted;
                default:
                    return MoveResult.Moved;
            }
        }
    }
}
