using System;

using UnityEngine;

using Cube.Level;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorGrid : MonoBehaviour
    {
        private LevelData _level = null;

        public void Init(LevelData level)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            _level = level;
            level.OnSizeChanged += OnLevelSizeChanged;
            OnLevelSizeChanged();
        }

        public void Fini()
        {
            LevelData level = _level;
            if (level != null)
            {
                level.OnSizeChanged -= OnLevelSizeChanged;
                _level = null;
            }
        }

        private void OnLevelSizeChanged()
        {
            Vector2 levelSize = (Vector2)_level.GetSize();
            MatchPositionWithLevelSize(levelSize);
            MatchScaleWithLevelSize(levelSize);
        }

        private void MatchPositionWithLevelSize(Vector2 levelSize)
        {
            Vector2 levelSizeHalf = levelSize * 0.5f;
            Vector3 position = transform.position;
            position.x = levelSizeHalf.x;
            position.y = levelSizeHalf.y;
            transform.position = position;
        }

        private void MatchScaleWithLevelSize(Vector2 levelSize)
        {
            Vector3 scale = transform.localScale;
            scale.x = levelSize.x;
            scale.y = levelSize.y;
            transform.localScale = scale;
        }
    }
}
