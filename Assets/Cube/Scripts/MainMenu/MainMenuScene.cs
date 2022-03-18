using UnityEngine;
using UnityEngine.UI;

using Cube.SceneManagement;

namespace Cube.MainMenu
{
    public sealed class MainMenuScene : GameScene
    {
        [SerializeField]
        private Button _playButton = null;

        public override void OnAfterActivate(object data) { }

        public override void OnBeforeDeactivate(object data) { }

        public override void OnAfterLoad(object data)
        {
            InitPlayButton();
        }

        public override void OnBeforeUnload(object data) { }

        private void InitPlayButton()
        {
            Button.ButtonClickedEvent onClick = _playButton.onClick;
            onClick.RemoveAllListeners();
            onClick.AddListener(MoveToGameplayScene);
        }

        private void MoveToGameplayScene()
        {
            GameSceneManager.DeactivateGameScene(this, null);
            GameSceneManager.ActivateGameScene(GameSceneManager.GetLoadedGameScene(SceneNames.Gameplay), null);
        }
    }
}
