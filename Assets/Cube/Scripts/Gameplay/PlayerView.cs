using System.Collections;

using UnityEngine;

namespace Cube.Gameplay
{
    public sealed class PlayerView : MonoBehaviour
    {
        public float MoveDuration = 0.5f;

        public bool Moving { get; private set; } = false;

        public void Init(Vector2Int position)
        {
            transform.position = new Vector3(position.x + 0.5f, 0.5f, position.y + 0.5f);
        }

        public void StartMove(Vector2Int direction)
        {
            if (Moving)
            {
                Debug.LogError("Already moving.");
                return;
            }

            StartCoroutine(Move(direction));
        }

        private IEnumerator Move(Vector2Int direction)
        {
            Moving = true;

            Vector3 direction3D = new Vector3(direction.x, 0f, direction.y);
            Vector3 axis = Vector3.Cross(Vector3.up, direction3D);
            Vector3 center = transform.position + ((direction3D + Vector3.down) * 0.5f);

            float angle = 0f;
            float time = 0f;
            while (time < MoveDuration)
            {
                float deltaTime = Time.deltaTime;
                float deltaAngle = Mathf.Min((deltaTime / MoveDuration) * 90f, 90f - angle);
                transform.RotateAround(center, axis, deltaAngle);
                angle += deltaAngle;
                time += deltaTime;
                yield return null;
            }
            transform.RotateAround(center, axis, 90f - angle);

            Moving = false;
        }
    }
}
