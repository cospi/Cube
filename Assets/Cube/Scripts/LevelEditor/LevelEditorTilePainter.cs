using System;

using UnityEngine;
using UnityEngine.EventSystems;

using Cube.Level;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorTilePainter
        : MonoBehaviour
        , IPointerDownHandler
        , IPointerUpHandler
        , IPointerEnterHandler
        , IPointerExitHandler
        , IDragHandler
    {
        public Camera Camera = null;

        [NonSerialized]
        public Tile Tile = default(Tile);

        private LevelData _level = null;
        private bool _pointerDown = false;
        private bool _paintActive = false;
        private Vector2Int _previousTilePosition = Vector2Int.zero;

        public void Init(LevelData level)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            _level = level;
            _pointerDown = false;
            _paintActive = false;
            _previousTilePosition = Vector2Int.zero;
        }

        public void Fini()
        {
            _level = null;
            _pointerDown = false;
            _paintActive = false;
            _previousTilePosition = Vector2Int.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
            _paintActive = true;
            PaintTile(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerDown = false;
            _paintActive = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_pointerDown)
            {
                _paintActive = true;
                PaintTile(eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _paintActive = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_paintActive)
            {
                Vector2Int tilePosition = GetEventDataTile(eventData);
                if (tilePosition != _previousTilePosition)
                {
                    PaintTile(tilePosition);
                }
            }
        }

        private void PaintTile(Vector2Int tilePosition)
        {
            LevelData level = _level;
            if ((level != null) && level.IsValidPosition(tilePosition))
            {
                level.SetTile(tilePosition, Tile);
            }
            _previousTilePosition = tilePosition;
        }

        private void PaintTile(PointerEventData eventData)
        {
            PaintTile(GetEventDataTile(eventData));
        }

        private Vector2Int GetEventDataTile(PointerEventData eventData)
        {
            Vector3 worldPosition = Camera.ScreenToWorldPoint(eventData.position);
            return new Vector2Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
        }
    }
}
