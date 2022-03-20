using UnityEngine;

namespace Cube.Utils
{
    public static class SpriteUtils
    {
        public static Rect GetSpriteUVRect(Sprite sprite)
        {
            // Starting from the opposite ends guarantees the Min/Max inside the loop will produce the correct result.
            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);
            foreach (Vector2 uv in sprite.uv)
            {
                min = Vector2.Min(min, uv);
                max = Vector2.Max(max, uv);
            }
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }
    }
}
