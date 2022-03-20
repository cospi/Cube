using System;
using System.Collections.Generic;

using UnityEngine;

using Cube.Level;

namespace Cube.LevelSelection
{
    public sealed class LevelSelectionLevelList : MonoBehaviour
    {
        [SerializeField]
        private LevelSelectionLevelItem _itemPrefab = null;
        [SerializeField]
        private Transform _content = null;

        private readonly List<LevelSelectionLevelItem> _items = new List<LevelSelectionLevelItem>();
        private TextAsset[] _levelAssets = null;
        private Action<TextAsset> _onSelected = null;

        public void Init(TextAsset[] levelAssets, Action<TextAsset> onSelected)
        {
            if (levelAssets == null)
            {
                throw new ArgumentNullException(nameof(levelAssets));
            }

            _levelAssets = levelAssets;
            _onSelected = onSelected;
            DestroyItems();
            CreateItems();
        }

        public void Fini()
        {
            DestroyItems();
        }

        private void DestroyItems()
        {
            List<LevelSelectionLevelItem> items = _items;
            foreach (LevelSelectionLevelItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
        }

        private void CreateItems()
        {
            List<LevelSelectionLevelItem> items = _items;
            LevelSelectionLevelItem itemPrefab = _itemPrefab;
            Transform content = _content;
            int count = _levelAssets.Length;
            for (int i = 0; i < count; ++i)
            {
                LevelSelectionLevelItem item = Instantiate(itemPrefab, content);
                item.Init(i, OnSelected);
                items.Add(item);
            }
        }

        private void OnSelected(int levelIndex)
        {
            _onSelected?.Invoke(_levelAssets[levelIndex]);
        }
    }
}
