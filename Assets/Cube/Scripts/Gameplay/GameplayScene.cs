using UnityEngine;
using UnityEngine.UI;

using Cube.SceneManagement;

namespace Cube.Gameplay
{
    public sealed class GameplayScene : GameScene
    {
        [SerializeField]
        private Button _exitButton = null;

        public override void OnAfterLoad(object data)
        {
            InitExitButton();
        }

        public override void OnBeforeUnload(object data) { }

        public override void OnAfterActivate(object data) { }

        public override void OnBeforeDeactivate(object data) { }

        private void InitExitButton()
        {
            Button.ButtonClickedEvent onClick = _exitButton.onClick;
            onClick.RemoveAllListeners();
            onClick.AddListener(MoveToMainMenuScene);
        }

        private void MoveToMainMenuScene()
        {
            GameSceneManager.DeactivateGameScene(this, null);
            GameSceneManager.ActivateGameScene(GameSceneManager.GetLoadedGameScene(SceneNames.MainMenu), null);
        }
    }
}
