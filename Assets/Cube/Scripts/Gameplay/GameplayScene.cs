using UnityEngine;
using UnityEngine.UI;

using Cube.Level;
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
            public readonly object ExitData = null;

            public ActivateData(LevelData level, GameScene exitScene, object exitData)
            {
                Level = level;
                ExitScene = exitScene;
                ExitData = exitData;
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
        private object _exitData = null;

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
            _exitData = activateData.ExitData;
            _levelView.Init(
                activateData.Level,
                Core.Instance.ServiceManager.GetService<TileSetService>().TileSet,
                Core.Instance.ServiceManager.GetService<TileSetService>().GameplayMaterial
            );
            _playerController.Init(activateData.Level);
            _controlPad.Init(_playerController);
        }

        public override void OnBeforeDeactivate(object data)
        {
            _levelView.Fini();
        }

        private void Exit()
        {
            GameSceneManager.MoveFromToGameScene(this, null, _exitScene, _exitData);
        }
    }
}
