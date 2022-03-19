using UnityEngine;

using Cube.SceneManagement;

namespace Cube.Start
{
    public sealed class StartScene : GameScene
    {
        private static readonly string[] NamesOfScenesToLoad =
        {
            SceneNames.MainMenu,
            SceneNames.Gameplay,
            SceneNames.LevelEditor
        };

        [SerializeField]
        private GameObject _loadingView = null;
        [SerializeField]
        private GameObject _errorView = null;

        private int _loadedSceneCount = 0;

        private void Awake()
        {
            GameSceneManager.InitFromActiveScene();
        }

        public override void OnAfterLoad(object data) { }

        public override void OnBeforeUnload(object data) { }

        public override void OnAfterActivate(object data)
        {
            LoadGameScenes();
        }

        public override void OnBeforeDeactivate(object data) { }

        private void LoadGameScenes()
        {
            SetError(false);
            _loadedSceneCount = 0;
            foreach (string sceneName in NamesOfScenesToLoad)
            {
                GameSceneManager.LoadGameSceneAsync(sceneName, null, OnGameSceneLoaded);
            }
        }

        private void OnGameSceneLoaded(GameScene gameScene)
        {
            if (gameScene != null)
            {
                OnGameSceneLoadSuccess();
            }
            else
            {
                OnGameSceneLoadFailed();
            }
        }

        private void OnGameSceneLoadSuccess()
        {
            if (++_loadedSceneCount == NamesOfScenesToLoad.Length)
            {
                MoveToMainMenuScene();
            }
        }

        private void OnGameSceneLoadFailed()
        {
            SetError(true);
        }

        private void MoveToMainMenuScene()
        {
            GameSceneManager.DeactivateGameScene(this, null);
            GameSceneManager.ActivateGameScene(GameSceneManager.GetLoadedGameScene(SceneNames.MainMenu), null);
            GameSceneManager.UnloadGameScene(this, null);
        }

        private void SetError(bool error)
        {
            _loadingView.SetActive(!error);
            _errorView.SetActive(error);
        }
    }
}
