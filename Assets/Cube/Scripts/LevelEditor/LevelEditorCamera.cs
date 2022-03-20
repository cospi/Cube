using UnityEngine;

using Cube.Utils;

namespace Cube.LevelEditor
{
    public sealed class LevelEditorCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera = null;
        [SerializeField]
        private RectTransform _bottomMenu = null;

        public void Init()
        {
            Camera camera = _camera;
            Transform cameraTransform = camera.transform;
            Vector3 position = CameraUtils.GetOrthoCameraExtents(camera);
            position.y -= RectTransformUtils.GetRectTransformHeightInOrthoCamera(_bottomMenu, camera);
            position.z = cameraTransform.position.z;
            cameraTransform.position = position;
        }
    }
}
