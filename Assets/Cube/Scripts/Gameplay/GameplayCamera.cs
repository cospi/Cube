using UnityEngine;

namespace Cube.Gameplay
{
    public sealed class GameplayCamera : MonoBehaviour
    {
        public Transform Target = null;
        public Vector2 OffsetXZ = new Vector2(0, -5f);
        public float PositionY = 5f;


        private void LateUpdate()
        {
            Transform target = Target;
            if (target != null)
            {
                Vector3 position = target.position;
                Vector2 offsetXZ = OffsetXZ;
                position.x += offsetXZ.x;
                position.z += offsetXZ.y;
                position.y = PositionY;
                transform.position = position;
            }
        }
    }
}
