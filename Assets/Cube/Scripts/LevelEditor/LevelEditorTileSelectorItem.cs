using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Cube.Level;
using Cube.Utils;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorTileSelectorItem : MonoBehaviour
    {
        [SerializeField]
        private Button _button = null;
        [SerializeField]
        private TextMeshProUGUI _text = null;
        [SerializeField]
        private GameObject _selectedIndicator = null;

        private int _index = 0;
        private Tile _tile = default(Tile);
        private Action<int, Tile> _onSelected = null;

        public void Init(int index, Tile tile, Action<int, Tile> onSelected)
        {
            _index = index;
            _tile = tile;
            _onSelected = onSelected;

            _text.text = tile.ToString();
            ButtonUtils.InitButton(_button, Select);
        }

        public void Select()
        {
            _onSelected?.Invoke(_index, _tile);
        }

        public void SetSelectedIndicatorActive(bool active)
        {
            _selectedIndicator.SetActive(active);
        }
    }
}
