using System;

using UnityEngine.Events;
using UnityEngine.UI;

namespace Cube.Utils
{
    public static class ButtonUtils
    {
        public static void InitButton(Button button, UnityAction onClicked)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }

            if (onClicked == null)
            {
                throw new ArgumentNullException(nameof(onClicked));
            }

            Button.ButtonClickedEvent onClick = button.onClick;
            onClick.RemoveAllListeners();
            onClick.AddListener(onClicked);
        }
    }
}
