using System.Collections.Generic;

using UnityEngine;

namespace Cube.Utils
{
    public static class GameObjectUtils
    {
        public static void SetGameObjectsActive(IEnumerable<GameObject> gameObjects, bool active)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(active);
            }
        }
    }
}
