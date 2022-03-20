using UnityEngine;

namespace Cube.Utils
{
    public static class CameraUtils
    {
        public static Vector3 GetOrthoCameraExtents(Camera camera)
        {
            Vector3 cameraExtents = default(Vector3);
            cameraExtents.y = camera.orthographicSize;
            cameraExtents.x = cameraExtents.y * camera.aspect;
            return cameraExtents;
        }
    }
}
