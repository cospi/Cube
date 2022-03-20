using UnityEngine;

namespace Cube.Level
{
    [CreateAssetMenu(fileName = "New World", menuName = "Cube/Level/World")]
    public sealed class World : ScriptableObject
    {
        public string Name = "";
        public TextAsset[] LevelAssets = { };
    }
}
