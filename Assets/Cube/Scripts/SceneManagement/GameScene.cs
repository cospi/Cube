using UnityEngine;

namespace Cube.SceneManagement
{
    public abstract class GameScene : MonoBehaviour
    {
        public abstract void OnAfterLoad(object data);
        public abstract void OnBeforeUnload(object data);
        public abstract void OnAfterActivate(object data);
        public abstract void OnBeforeDeactivate(object data);
    }
}
