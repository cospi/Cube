using System;

using UnityEngine;

using Cube.Graphics;
using Cube.Level;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorLevelView : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _meshRenderer = null;
        [SerializeField]
        private Material _material = null;
        [SerializeField]
        private LevelEditorTileSet _tileSet = null;
        [SerializeField]
        private RectBatch _rectBatch = null;

        private LevelData _level = null;
        private Material _runtimeMaterial = null;

        public void Init(LevelData level)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            _level = level;
            InitRectBatch();
            InitRuntimeMaterial();
            level.OnSizeChanged += OnLevelSizeChanged;
            level.OnTileChanged += OnLevelTileChanged;
        }

        public void Fini()
        {
            _rectBatch.Fini();
            DestroyRuntimeMaterialIfExists();
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

        private void RefreshRectBatch(RectBatch rectBatch, TileData[,] tiles, Vector2Int size)
        {
            LevelEditorTileSet tileSet = _tileSet;
            rectBatch.Begin();
            for (int y = 0; y < size.y; ++y)
            {
                float yF = (float)y;
                for (int x = 0; x < size.x; ++x)
                {
                    Sprite tileSprite = tileSet.GetTileSprite(tiles[y, x].Type);
                    if (tileSprite != null)
                    {
                        float xF = (float)x;
                        rectBatch.PushRect(new Rect(xF, yF, 1f, 1f), GetSpriteUVRect(tileSprite));
                    }
                }
            }
            rectBatch.End();
        }

        private static Rect GetSpriteUVRect(Sprite sprite)
        {
            // Starting from the opposite ends guarantees the Min/Max inside the loop will produce the correct result.
            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);
            foreach (Vector2 uv in sprite.uv)
            {
                min = Vector2.Min(min, uv);
                max = Vector2.Max(min, uv);
            }
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
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

        private void InitRuntimeMaterial()
        {
            Material runtimeMaterial = _runtimeMaterial;
            if (runtimeMaterial == null)
            {
                MeshRenderer meshRenderer = _meshRenderer;
                runtimeMaterial = new Material(_material);
                meshRenderer.sharedMaterial = runtimeMaterial;
                _runtimeMaterial = runtimeMaterial;
            }
            runtimeMaterial.mainTexture = _tileSet.GetTexture();
        }

        private void DestroyRuntimeMaterialIfExists()
        {
            Material runtimeMaterial = _runtimeMaterial;
            if (runtimeMaterial != null)
            {
                Destroy(runtimeMaterial);
                _runtimeMaterial = null;
            }
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
