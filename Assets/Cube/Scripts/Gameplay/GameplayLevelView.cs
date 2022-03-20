using System;

using UnityEngine;

using Cube.Level;

namespace Cube.Gameplay
{
    public sealed class GameplayLevelView : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter _meshFilter = null;
        [SerializeField]
        private MeshRenderer _meshRenderer = null;

        private Mesh _mesh = null;

        public void Init(LevelData level, TileSet tileSet, Material material)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            DestroyMeshIfExists();
            Mesh mesh = GameplayLevelMeshCreator.CreateLevelMesh(level, tileSet);
            _meshFilter.mesh = mesh;
            _mesh = mesh;
            _meshRenderer.material = material;
        }

        public void Fini()
        {
            DestroyMeshIfExists();
        }

        private void DestroyMeshIfExists()
        {
            Mesh mesh = _mesh;
            if (mesh != null)
            {
                Destroy(mesh);
                _mesh = null;
            }
        }
    }
}
