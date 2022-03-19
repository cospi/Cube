using UnityEngine;
using UnityEngine.UI;

using Cube.Level;
using Cube.SceneManagement;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorScene : GameScene
    {
        [SerializeField]
        private Button _exitButton = null;
        [SerializeField]
        private LevelEditorLevelView _levelView = null;
        [SerializeField]
        private LevelEditorGrid _grid = null;
        [SerializeField]
        private LevelEditorLevelSizeMenu _levelSizeMenu = null;
        [SerializeField]
        private LevelEditorToolSelector _toolSelector = null;
        [SerializeField]
        private LevelEditorTileSelector _tileSelector = null;
        [SerializeField]
        private LevelEditorTilePainter _tilePainter = null;

        public override void OnAfterLoad(object data)
        {
            InitExitButton();
            _tileSelector.Init(_tilePainter);
        }

        public override void OnBeforeUnload(object data) { }

        public override void OnAfterActivate(object data)
        {
            LevelData level = new LevelData(new Vector2Int(10, 10));
            _levelView.Init(level);
            _grid.Init(level);
            _levelSizeMenu.Init(level);
            _toolSelector.SelectTool(0);
            _tileSelector.SelectTile(0);
            _tilePainter.Init(level);
        }

        public override void OnBeforeDeactivate(object data)
        {
            _levelView.Fini();
            _grid.Fini();
            _levelSizeMenu.Fini();
        }

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
