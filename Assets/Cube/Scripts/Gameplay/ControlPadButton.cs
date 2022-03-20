using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Cube.Gameplay
{
    public sealed class ControlPadButton
        : MonoBehaviour
        , IPointerDownHandler
        , IPointerUpHandler
        , IPointerEnterHandler
        , IPointerExitHandler
    {
        public Vector2Int Direction = Vector2Int.zero;

        private PlayerController _playerController = null;
        private bool _pointerDown = false;

        public void Init(PlayerController playerController)
        {
            if (playerController == null)
            {
                throw new ArgumentNullException(nameof(playerController));
            }

            _playerController = playerController;
            _pointerDown = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
            ApplyDirection();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerDown = false;
            ResetDirection();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_pointerDown)
            {
                ApplyDirection();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_pointerDown)
            {
                ResetDirection();
            }
        }

        private void ApplyDirection()
        {
            PlayerController playerController = _playerController;
            if (playerController != null)
            {
                playerController.Direction = Direction;
            }
        }

        private void ResetDirection()
        {
            PlayerController playerController = _playerController;
            if (playerController != null)
            {
                playerController.Direction.Set(0, 0);
            }
        }
    }
}
