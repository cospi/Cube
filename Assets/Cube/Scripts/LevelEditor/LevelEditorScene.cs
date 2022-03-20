
using System.IO;

using UnityEngine;
using UnityEngine.UI;

using Cube.Gameplay;
using Cube.Level;
using Cube.SceneManagement;
using Cube.Utils;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorScene : GameScene
    {
        public sealed class ActivateData
        {
            public readonly bool TriggerInit = false;

            public ActivateData(bool triggerInit)
            {
                TriggerInit = triggerInit;
            }
        }

        public sealed class DeactivateData
        {
            public readonly bool TriggerFini = false;

            public DeactivateData(bool triggerFini)
            {
                TriggerFini = triggerFini;
            }
        }

        [SerializeField]
        private Button _exitButton = null;
        [SerializeField]
        private Button _loadButton = null;
        [SerializeField]
        private Button _saveButton = null;
        [SerializeField]
        private Button _playTestButton = null;
        [SerializeField]
        private LevelEditorCamera _camera = null;
        [SerializeField]
        private LevelEditorLevelView _levelView = null;
        [SerializeField]
        private LevelEditorGrid _grid = null;
        [SerializeField]
        private LevelEditorLevelSizeMenu _levelSizeMenu = null;
        [SerializeField]
        private LevelEditorTilePainter _tilePainter = null;
        [SerializeField]
        private LevelEditorToolSelector _toolSelector = null;
        [SerializeField]
        private LevelEditorTileSelector _tileSelector = null;
        [SerializeField]
        private LevelEditorLoadMenu _loadMenu = null;
        [SerializeField]
        private LevelEditorSaveMenu _saveMenu = null;

        private LevelData _level = null;

        public override void OnAfterLoad(object data)
        {
            ButtonUtils.InitButton(_exitButton, MoveToMainMenuScene);
            ButtonUtils.InitButton(_loadButton, () => _loadMenu.Show(GetLevelDirectory(), OnLevelLoaded));
            ButtonUtils.InitButton(_saveButton, () => _saveMenu.Show(GetLevelDirectory(), _level));
            ButtonUtils.InitButton(_playTestButton, MoveToGameplayScene);
            _tileSelector.Init(_tilePainter);
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

            if (activateData.TriggerInit)
            {
                _camera.Init();
                LevelData level = new LevelData(new Vector2Int(10, 10));
                _level = level;
                _levelView.Init(
                    level,
                    Core.Instance.ServiceManager.GetService<TileSetService>().TileSet,
                    Core.Instance.ServiceManager.GetService<TileSetService>().LevelEditorMaterial
                );
                _grid.Init(level);
                _levelSizeMenu.Init(level);
                _tilePainter.Init(level);
                _toolSelector.SelectTool(0);
                _tileSelector.SelectTile(0);
            }
        }

        public override void OnBeforeDeactivate(object data)
        {
            DeactivateData deactivateData = data as DeactivateData;
            if (deactivateData == null)
            {
                Debug.LogError("Invalid data.");
                return;
            }

            if (deactivateData.TriggerFini)
            {
                _level = null;
                _levelView.Fini();
                _grid.Fini();
                _levelSizeMenu.Fini();
                _tilePainter.Fini();
            }
        }

        private void MoveToMainMenuScene()
        {
            GameSceneManager.MoveFromToGameScene(
                this,
                new LevelEditorScene.DeactivateData(true),
                SceneNames.MainMenu,
                null
            );
        }

        private void MoveToGameplayScene()
        {
            GameSceneManager.MoveFromToGameScene(
                this,
                new LevelEditorScene.DeactivateData(false),
                SceneNames.Gameplay,
                new GameplayScene.ActivateData(_level, this, new LevelEditorScene.ActivateData(false))
            );
        }

        private void OnLevelLoaded(LevelData level)
        {
            // Copy level instead of re-assigning to make sure level change listeners will continue to function.
            _level.Copy(level);
        }

        private static string GetLevelDirectory()
        {
            return Path.Combine(Application.persistentDataPath, "Levels");
        }
    }
}
