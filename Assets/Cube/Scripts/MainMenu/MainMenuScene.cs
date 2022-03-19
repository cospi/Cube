using UnityEngine;
using UnityEngine.UI;

using Cube.SceneManagement;

namespace Cube.MainMenu
{
    public sealed class MainMenuScene : GameScene
    {
        [SerializeField]
        private Button _playButton = null;
        [SerializeField]
        private Button _levelEditorButton = null;

        public override void OnAfterLoad(object data)
        {
            InitPlayButton();
            InitLevelEditorButton();
        }

        public override void OnBeforeUnload(object data) { }

        public override void OnAfterActivate(object data) { }

        public override void OnBeforeDeactivate(object data) { }

        private void InitPlayButton()
        {
            Button.ButtonClickedEvent onClick = _playButton.onClick;
            onClick.RemoveAllListeners();
            onClick.AddListener(MoveToGameplayScene);
        }

        private void InitLevelEditorButton()
        {
            Button.ButtonClickedEvent onClick = _levelEditorButton.onClick;
            onClick.RemoveAllListeners();
            onClick.AddListener(MoveToLevelEditorScene);
        }

        private void MoveToGameplayScene()
        {
            GameSceneManager.DeactivateGameScene(this, null);
            GameSceneManager.ActivateGameScene(GameSceneManager.GetLoadedGameScene(SceneNames.Gameplay), null);
        }

        private void MoveToLevelEditorScene()
        {
            GameSceneManager.DeactivateGameScene(this, null);
            GameSceneManager.ActivateGameScene(GameSceneManager.GetLoadedGameScene(SceneNames.LevelEditor), null);
        }
    }
}
