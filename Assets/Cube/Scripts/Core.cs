using UnityEngine;

using Cube.Level;
using Cube.SceneManagement;
using Cube.ServiceManagement;

namespace Cube
{
    public sealed class Core : MonoBehaviour
    {
        [SerializeField]
        private TileSet _tileSet = null;
        [SerializeField]
        private Material _gameplayMaterialAsset = null;
        [SerializeField]
        private Material _levelEditorMaterialAsset = null;

        public static Core Instance { get; private set; } = null;
        public readonly ServiceManager ServiceManager = new ServiceManager();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"{nameof(Core)} instance already exists.");
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);

            GameSceneManager.InitFromActiveScene();
            ServiceManager.SetService(new TileSetService(_tileSet, _gameplayMaterialAsset, _levelEditorMaterialAsset));
        }
    }
}
