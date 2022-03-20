using System;

using UnityEngine;

namespace Cube.Gameplay
{
    public sealed class ControlPad : MonoBehaviour
    {
        [SerializeField]
        private ControlPadButton[] _buttons = { };

        public void Init(PlayerController playerController)
        {
            if (playerController == null)
            {
                throw new ArgumentNullException(nameof(playerController));
            }

            foreach (ControlPadButton button in _buttons)
            {
                button.Init(playerController);
            }
        }
    }
}
