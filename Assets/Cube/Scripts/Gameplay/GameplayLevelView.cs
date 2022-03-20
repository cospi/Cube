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
        [SerializeField]
        private GameObject _goal = null;

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
            InitGoal(level);
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

        private void InitGoal(LevelData level)
        {
            Vector2Int goalPosition;
            bool goalActive = level.TryGetTilePosition(Tile.Goal, out goalPosition);
            GameObject goal = _goal;
            goal.SetActive(goalActive);
            if (goalActive)
            {
                goal.transform.position = new Vector3(goalPosition.x + 0.5f, 0f, goalPosition.y + 0.5f);
            }
        }
    }
}
