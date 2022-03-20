using System;

using UnityEngine;
using UnityEngine.UI;

using Cube.Level;
using Cube.Utils;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorLoadMenu : MonoBehaviour
    {
        [SerializeField]
        private LevelEditorFileMenuList _list = null;
        [SerializeField]
        private Button _exitButton = null;

        private string _directory = null;
        private Action<LevelData> _onLoaded = null;

        private void Awake()
        {
            ButtonUtils.InitButton(_exitButton, Hide);
        }

        public void Show(string directory, Action<LevelData> onLoaded)
        {
            _directory = directory;
            _onLoaded = onLoaded;
            _list.Init(directory, Load);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _directory = null;
            _onLoaded = null;
            _list.Fini();
        }

        private void Load(string path)
        {
            LevelData level = LevelLoader.LoadLevel(path);
            if (level == null)
            {
                // TODO: Inform player about error.
                return;
            }

            _onLoaded?.Invoke(level);
            Hide();
        }
    }
}
