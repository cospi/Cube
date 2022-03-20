using UnityEngine;
using UnityEngine.UI;

using Cube.Gameplay;
using Cube.Level;
using Cube.SceneManagement;
using Cube.Utils;

namespace Cube.LevelSelection
{
    public sealed class LevelSelectionScene : GameScene
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
        private World[] _worlds = null;
        [SerializeField]
        private LevelSelectionWorldMenu _worldMenu = null;

        public override void OnAfterLoad(object data)
        {
            ButtonUtils.InitButton(_exitButton, MoveToMainMenuScene);
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
                _worldMenu.Init(_worlds, OnLevelSelected);
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
                _worldMenu.Fini();
            }
        }

        private void MoveToMainMenuScene()
        {
            GameSceneManager.MoveFromToGameScene(this, new DeactivateData(true), SceneNames.MainMenu, null);
        }

        private void OnLevelSelected(TextAsset levelAsset)
        {
            LevelData level = LevelLoader.LoadLevel(levelAsset);
            if (level == null)
            {
                // TODO: Inform player about error.
                return;
            }

            GameSceneManager.MoveFromToGameScene(
                this,
                new DeactivateData(false),
                SceneNames.Gameplay,
                new GameplayScene.ActivateData(level, this, new ActivateData(false))
            );
        }
    }
}
