using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Cube.Level;
using Cube.Utils;

namespace Cube.LevelSelection
{
    public sealed class LevelSelectionWorldMenu : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _worldNameText = null;
        [SerializeField]
        private LevelSelectionLevelList _levelList = null;
        [SerializeField]
        private Button _nextWorldButton = null;
        [SerializeField]
        private Button _previousWorldButton = null;

        private World[] _worlds = null;
        private Action<TextAsset> _onLevelSelected = null;
        private int _worldIndex = 0;

        public void Init(World[] worlds, Action<TextAsset> onLevelSelected)
        {
            if (worlds == null)
            {
                throw new ArgumentNullException(nameof(worlds));
            }

            if (worlds.Length == 0)
            {
                throw new ArgumentException($"{nameof(worlds)} is empty.", nameof(worlds));
            }

            _worlds = worlds;
            _onLevelSelected = onLevelSelected;
            SetActiveWorld(0);
            ButtonUtils.InitButton(_nextWorldButton, () => SwitchWorld(1));
            ButtonUtils.InitButton(_previousWorldButton, () => SwitchWorld(-1));
        }

        public void Fini()
        {
            _levelList.Fini();
        }

        private void SetActiveWorld(int worldIndex)
        {
            World world = _worlds[worldIndex];
            _worldNameText.text = world.Name;
            _levelList.Init(world.LevelAssets, OnLevelSelected);
            _worldIndex = worldIndex;
            UpdateSwitchWorldButtonsActive();
        }

        private void SwitchWorld(int direction)
        {
            SetActiveWorld(_worldIndex + direction);
        }

        private void OnLevelSelected(TextAsset levelAsset)
        {
            _onLevelSelected?.Invoke(levelAsset);
        }

        private void UpdateSwitchWorldButtonsActive()
        {
            int worldIndex = _worldIndex;
            _nextWorldButton.gameObject.SetActive(worldIndex < (_worlds.Length - 1));
            _previousWorldButton.gameObject.SetActive(worldIndex > 0);
        }
    }
}
