using System;

using UnityEngine;

using Cube.Graphics;
using Cube.Level;
using Cube.Utils;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorLevelView : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _meshRenderer = null;
        [SerializeField]
        private RectBatch _rectBatch = null;

        private LevelData _level = null;
        private TileSet _tileSet = null;

        public void Init(LevelData level, TileSet tileSet, Material material)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            _level = level;
            _tileSet = tileSet;
            InitRectBatch();
            level.OnSizeChanged += OnLevelSizeChanged;
            level.OnTileChanged += OnLevelTileChanged;
            _meshRenderer.sharedMaterial = material;
        }

        public void Fini()
        {
            _rectBatch.Fini();
            UnsubscribeFromLevel();
        }

        private void InitRectBatch()
        {
            LevelData level = _level;
            Vector2Int size = level.GetSize();
            RectBatch rectBatch = _rectBatch;
            rectBatch.Init(size.x * size.y);
            RefreshRectBatch(rectBatch, level.Tiles, size);
        }

        private void RefreshRectBatch(RectBatch rectBatch, Tile[,] tiles, Vector2Int size)
        {
            TileSet tileSet = _tileSet;
            rectBatch.Begin();
            for (int y = 0; y < size.y; ++y)
            {
                float yF = (float)y;
                for (int x = 0; x < size.x; ++x)
                {
                    Sprite tileSprite = tileSet.GetTileSprite(tiles[y, x]);
                    if (tileSprite != null)
                    {
                        float xF = (float)x;
                        rectBatch.PushRect(new Rect(xF, yF, 1f, 1f), SpriteUtils.GetSpriteUVRect(tileSprite));
                    }
                }
            }
            rectBatch.End();
        }

        private void OnLevelSizeChanged()
        {
            // Size change requires re-configuring batch capacity.
            InitRectBatch();
        }

        private void OnLevelTileChanged()
        {
            // Tile change doesn't require re-configuring batch capacity.
            LevelData level = _level;
            RefreshRectBatch(_rectBatch, level.Tiles, level.GetSize());
        }

        private void UnsubscribeFromLevel()
        {
            LevelData level = _level;
            if (level != null)
            {
                level.OnSizeChanged -= OnLevelSizeChanged;
                level.OnTileChanged -= OnLevelTileChanged;
                _level = null;
            }
        }
    }
}
