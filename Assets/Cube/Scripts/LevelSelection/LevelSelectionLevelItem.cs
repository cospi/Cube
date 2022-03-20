using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Cube.Utils;

namespace Cube.LevelSelection
{
    public sealed class LevelSelectionLevelItem : MonoBehaviour
    {
        [SerializeField]
        private Button _button = null;
        [SerializeField]
        private TextMeshProUGUI _text = null;

        private int _index = 0;
        private Action<int> _onSelected = null;

        public void Init(int index, Action<int> onSelected)
        {
            _index = index;
            _onSelected = onSelected;
            ButtonUtils.InitButton(_button, Select);
            _text.text = (index + 1).ToString();
        }

        private void Select()
        {
            _onSelected?.Invoke(_index);
        }
    }
}
