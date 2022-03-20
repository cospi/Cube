using System;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Cube.Utils;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorFileMenuItem : MonoBehaviour
    {
        [SerializeField]
        private Button _selectButton = null;
        [SerializeField]
        private Button _deleteButton = null;
        [SerializeField]
        private TextMeshProUGUI _filenameText = null;

        public void Init(string path, Action<string> onSelectClicked, Action<string> onDeleteClicked)
        {
            InitButton(_selectButton, path, onSelectClicked);
            InitButton(_deleteButton, path, onDeleteClicked);
            _filenameText.text = Path.GetFileNameWithoutExtension(path);
        }

        private static void InitButton(Button button, string path, Action<string> onClicked)
        {
            ButtonUtils.InitButton(button, () => onClicked?.Invoke(path));
        }
    }
}
