using UnityEngine;
using UnityEngine.UI;

using Cube.Level;
using Cube.Result;
using Cube.SceneManagement;
using Cube.Utils;

namespace Cube.Gameplay
{
    public sealed class GameplayScene : GameScene
    {
        public sealed class ActivateData
        {
            public readonly LevelData Level = null;
            public readonly GameScene ExitScene = null;
            public readonly object ExitSceneActivateData = null;

            public ActivateData(LevelData level, GameScene exitScene, object exitSceneActivateData)
            {
                Level = level;
                ExitScene = exitScene;
                ExitSceneActivateData = exitSceneActivateData;
            }
        }

        [SerializeField]
        private Button _exitButton = null;
        [SerializeField]
        private GameplayLevelView _levelView = null;
        [SerializeField]
        private PlayerController _playerController = null;
        [SerializeField]
        private ControlPad _controlPad = null;

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
            InitLevelView(activateData.Level);
            _playerController.Init(activateData.Level, OnLevelEnded);
            _controlPad.Init(_playerController);
        }

        public override void OnBeforeDeactivate(object data)
        {
            _levelView.Fini();
            _playerController.Fini();
        }

        private void InitLevelView(LevelData level)
        {
            TileSetService tileSetService = Core.Instance.ServiceManager.GetService<TileSetService>();
            _levelView.Init(level, tileSetService.TileSet, tileSetService.GameplayMaterial);
        }

        private void OnLevelEnded(bool levelCompleted)
        {
            GameSceneManager.MoveFromToGameScene(
                this,
                null,
                SceneNames.Result,
                new ResultScene.ActivateData(levelCompleted, _exitScene, _exitSceneActivateData)
            );
        }

        private void Exit()
        {
            GameSceneManager.MoveFromToGameScene(this, null, _exitScene, _exitSceneActivateData);
        }
    }
}
