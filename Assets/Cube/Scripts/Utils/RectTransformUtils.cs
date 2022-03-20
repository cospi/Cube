using UnityEngine;

namespace Cube.Utils
{
    public static class RectTransformUtils
    {
        private static Vector3[] TmpVector3x4 = new Vector3[4];

        public static float GetRectTransformHeightInOrthoCamera(RectTransform menu, Camera camera)
        {
            float scale = (camera.orthographicSize * 2f) / Screen.height;
            // Use a statically allocated array to prevent excessive runtime allocations.
            Vector3[] corners = TmpVector3x4;
            menu.GetWorldCorners(corners);
            return (corners[1].y * scale) - (corners[0].y * scale);
        }
    }
}
