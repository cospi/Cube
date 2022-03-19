using UnityEngine;
using UnityEngine.EventSystems;

namespace Cube.UI
{
    public sealed class DragMovement : MonoBehaviour, IDragHandler
    {
        public Transform Target = null;
        public float Speed = 0.01f;

        public void OnDrag(PointerEventData eventData)
        {
            Transform target = Target;
            if (target != null)
            {
                // Translate by negative delta, because it feels intuitive for dragging.
                target.Translate(-eventData.delta * Speed);
            }
        }
    }
}
