using UnityEngine;
using UnityEngine.UI;

using Cube.SceneManagement;
using Cube.Utils;

namespace Cube.Result
{
    public sealed class ResultScene : GameScene
    {
        public sealed class ActivateData
        {
            public readonly bool LevelCompleted = false;
            public readonly GameScene ExitScene = null;
            public readonly object ExitSceneActivateData = null;

            public ActivateData(bool levelCompleted, GameScene exitScene, object exitSceneActivateData)
            {
                LevelCompleted = levelCompleted;
                ExitScene = exitScene;
                ExitSceneActivateData = exitSceneActivateData;
            }
        }

        [SerializeField]
        private Button _exitButton = null;
        [SerializeField]
        private ResultView _resultView = null;

        private GameScene _exitScene = null;
        private object _exitSceneActivateData = null;

        public override void OnAfterLoad(object data)
        {
            ButtonUtils.InitButton(_exitButton, Exit);
        }

        public override void OnBeforeUnload(object data) { }

        public override void OnAfterActivate(object data)
        {
            ActivateData activateData = data as ActivateData;
            if (activateData == null)
            {
                Debug.LogError("Invalid data.");
                return;
            }

            _exitScene = activateData.ExitScene;
            _exitSceneActivateData = activateData.ExitSceneActivateData;
            _resultView.Init(activateData.LevelCompleted);
        }

        public override void OnBeforeDeactivate(object data) { }

        private void Exit()
        {
            GameSceneManager.MoveFromToGameScene(this, null, _exitScene, _exitSceneActivateData);
        }
    }
}
