using System;
using System.Collections.Generic;

using UnityEngine;

using Cube.Level;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorTileSelector : MonoBehaviour
    {
        [SerializeField]
        private LevelEditorTileSelectorItem _itemPrefab = null;
        [SerializeField]
        private Transform _content = null;

        private List<LevelEditorTileSelectorItem> _items = null;
        private LevelEditorTilePainter _tilePainter = null;

        public void Init(LevelEditorTilePainter tilePainter)
        {
            if (tilePainter == null)
            {
                throw new ArgumentNullException(nameof(tilePainter));
            }

            LevelEditorTileSelectorItem itemPrefab = _itemPrefab;
            Transform content = _content;
            Array tiles = Enum.GetValues(typeof(Tile));
            int count = tiles.Length;
            List<LevelEditorTileSelectorItem> items = new List<LevelEditorTileSelectorItem>(count);
            for (int i = 0; i < count; ++i)
            {
                LevelEditorTileSelectorItem item = Instantiate(itemPrefab, content);
                item.Init(i, (Tile)tiles.GetValue(i), OnItemSelected);
                items.Add(item);
            }
            _items = items;
            _tilePainter = tilePainter;
        }

        public void SelectTile(int tileIndex)
        {
            List<LevelEditorTileSelectorItem> items = _items;
            if (items == null)
            {
                throw new InvalidOperationException($"{nameof(Init)} has not been executed successfully.");
            }

            if ((tileIndex < 0) || (tileIndex >= items.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(tileIndex));
            }

            items[tileIndex].Select();
        }

        private void OnItemSelected(int itemIndex, Tile tile)
        {
            UpdateSelectedIndicators(itemIndex);
            _tilePainter.Tile = tile;
        }

        private void UpdateSelectedIndicators(int selectedIndex)
        {
            List<LevelEditorTileSelectorItem> items = _items;
            int count = items.Count;
            for (int i = 0; i < count; ++i)
            {
                items[i].SetSelectedIndicatorActive(i == selectedIndex);
            }
        }
    }
}
