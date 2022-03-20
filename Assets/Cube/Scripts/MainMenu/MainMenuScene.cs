using UnityEngine;
using UnityEngine.UI;

using Cube.LevelEditor;
using Cube.LevelSelection;
using Cube.SceneManagement;
using Cube.Utils;

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
            ButtonUtils.InitButton(_playButton, MoveToLevelSelectionScene);
            ButtonUtils.InitButton(_levelEditorButton, MoveToLevelEditorScene);
        }

        public override void OnBeforeUnload(object data) { }

        public override void OnAfterActivate(object data) { }

        public override void OnBeforeDeactivate(object data) { }

        private void MoveToLevelSelectionScene()
        {
            GameSceneManager.MoveFromToGameScene(
                this,
                null,
                SceneNames.LevelSelection,
                new LevelSelectionScene.ActivateData(true)
            );
        }

        private void MoveToLevelEditorScene()
        {
            GameSceneManager.MoveFromToGameScene(
                this,
                null,
                SceneNames.LevelEditor,
                new LevelEditorScene.ActivateData(true)
            );
        }
    }
}
