using System;

using UnityEngine;

using TMPro;

using Cube.Level;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorLevelSizeMenu : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int _maxLevelSize = new Vector2Int(256, 256);
        [SerializeField]
        private TMP_InputField _widthInputField = null;
        [SerializeField]
        private TMP_InputField _heightInputField = null;

        private LevelData _level = null;

        private void Awake()
        {
            InitInputField(_widthInputField, 0);
            InitInputField(_heightInputField, 1);
        }

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
            Vector2Int levelSize = _level.GetSize();
            _widthInputField.text = levelSize.x.ToString();
            _heightInputField.text = levelSize.y.ToString();
        }

        private void InitInputField(TMP_InputField inputField, int sizeComponentIndex)
        {
            TMP_InputField.SubmitEvent onEndEdit = inputField.onEndEdit;
            onEndEdit.RemoveAllListeners();
            onEndEdit.AddListener(value =>
            {
                LevelData level = _level;
                if (level != null)
                {
                    Vector2Int size = level.GetSize();
                    if (int.TryParse(value, out int valueInt))
                    {
                        size[sizeComponentIndex] = Mathf.Clamp(valueInt, 1, _maxLevelSize[sizeComponentIndex]);
                        level.SetSize(size);
                    }
                    inputField.text = size[sizeComponentIndex].ToString();
                }
            });
        }
    }
}
