using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Cube.Level;

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
        private TileType _tileType = default(TileType);
        private Action<int, TileType> _onSelected = null;

        public void Init(int index, TileType tileType, Action<int, TileType> onSelected)
        {
            _index = index;
            _tileType = tileType;
            _onSelected = onSelected;

            _text.text = tileType.ToString();
            Button.ButtonClickedEvent onClick = _button.onClick;
            onClick.RemoveAllListeners();
            onClick.AddListener(Select);
        }

        public void Select()
        {
            _onSelected?.Invoke(_index, _tileType);
        }

        public void SetSelectedIndicatorActive(bool active)
        {
            _selectedIndicator.SetActive(active);
        }
    }
}
