using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

using Cube.Level;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorFileMenuList : MonoBehaviour
    {
        [SerializeField]
        private LevelEditorFileMenuItem _itemPrefab = null;
        [SerializeField]
        private Transform _content = null;

        private readonly List<LevelEditorFileMenuItem> _items = new List<LevelEditorFileMenuItem>();
        private string _directory = null;
        private Action<string> _onSelected = null;

        public void Init(string directory, Action<string> onSelected)
        {
            _directory = directory;
            _onSelected = onSelected;
            RefreshItems();
        }

        public void Fini()
        {
            _directory = null;
            _onSelected = null;
            DestroyItems();
        }

        private void RefreshItems()
        {
            DestroyItems();
            CreateItems();
        }

        private void DestroyItems()
        {
            List<LevelEditorFileMenuItem> items = _items;
            foreach (LevelEditorFileMenuItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
        }

        private void CreateItems()
        {
            string[] paths = GetPaths(_directory);
            if (paths == null)
            {
                return;
            }

            int count = paths.Length;
            if (count == 0)
            {
                return;
            }

            List<LevelEditorFileMenuItem> items = _items;
            LevelEditorFileMenuItem itemPrefab = _itemPrefab;
            Transform content = _content;
            Action<string> onSelected = _onSelected;
            foreach (string path in paths)
            {
                LevelEditorFileMenuItem item = Instantiate(itemPrefab, content);
                item.Init(path, onSelected, DeleteLevel);
                items.Add(item);
            }
        }

        private static string[] GetPaths(string directory)
        {
            string[] paths = null;
            try
            {
                if (Directory.Exists(directory))
                {
                    paths = Directory.GetFiles(directory);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return paths;
        }

        private void DeleteLevel(string path)
        {
            if (!LevelDeleter.DeleteLevel(path))
            {
                // TODO: Inform player about error.
                return;
            }

            RefreshItems();
        }
    }
}
