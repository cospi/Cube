using System.IO;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Cube.Level;
using Cube.Utils;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorSaveMenu : MonoBehaviour
    {
        [SerializeField]
        private LevelEditorFileMenuList _list = null;
        [SerializeField]
        private TMP_InputField _filenameInputField = null;
        [SerializeField]
        private Button _saveButton = null;
        [SerializeField]
        private Button _exitButton = null;

        private string _directory = null;
        private LevelData _level = null;

        private void Awake()
        {
            ButtonUtils.InitButton(_saveButton, Save);
            ButtonUtils.InitButton(_exitButton, Hide);
        }

        public void Show(string directory, LevelData level)
        {
            _directory = directory;
            _level = level;
            _list.Init(directory, OnFileSelected);
            _filenameInputField.text = "";
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _directory = null;
            _level = null;
            _list.Fini();
        }

        private void OnFileSelected(string path)
        {
            _filenameInputField.text = Path.GetFileNameWithoutExtension(path);
        }

        private void Save()
        {
            string filename = _filenameInputField.text;
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            if (!LevelSaver.SaveLevel(_level, Path.Combine(_directory, filename + ".bytes")))
            {
                // TODO: Inform player about error.
                return;
            }

            Hide();
        }
    }
}
