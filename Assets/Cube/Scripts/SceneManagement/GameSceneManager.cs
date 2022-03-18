using System;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cube.SceneManagement
{
    public static class GameSceneManager
    {
        public static void InitFromActiveScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            GameScene gameScene = GetGameSceneFromScene(activeScene);
            if (gameScene == null)
            {
                Debug.LogError($"Active scene is missing the {nameof(GameScene)} component.");
                return;
            }
            gameScene.gameObject.SetActive(true);
            gameScene.OnAfterActivate(null);
        }

        public static void LoadGameSceneAsync(string sceneName, object data, Action<GameScene> onLoaded)
        {
            if (sceneName == null)
            {
                throw new ArgumentNullException(nameof(sceneName));
            }

            Debug.Log($"Loading scene {sceneName}...");

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (loadOperation == null)
            {
                Debug.LogError($"Starting loading of scene {sceneName} failed.");
                onLoaded?.Invoke(null);
                return;
            }

            loadOperation.completed += _ =>
            {
                Scene scene = SceneManager.GetSceneByName(sceneName);
                if (!scene.IsValid())
                {
                    Debug.LogError($"Loading scene {sceneName} failed.");
                    onLoaded?.Invoke(null);
                    return;
                }

                GameScene gameScene = GetGameSceneFromScene(scene);
                if (gameScene == null)
                {
                    Debug.LogError($"Scene {sceneName} is missing the {nameof(GameScene)} component.");
                    // Deactivate to prevent artifacts during unloading.
                    SetSceneRootObjectsActive(scene, false);
                    SceneManager.UnloadSceneAsync(scene);
                    onLoaded?.Invoke(null);
                    return;
                }

                Debug.Log($"Loaded scene {sceneName}.");
                // If activation is desired, client code should call ActivateGameScene upon receiving the callback.
                gameScene.gameObject.SetActive(false);
                gameScene.OnAfterLoad(data);
                onLoaded?.Invoke(gameScene);
            };
        }

        public static void UnloadGameScene(GameScene gameScene, object data)
        {
            if (gameScene == null)
            {
                throw new ArgumentNullException(nameof(gameScene));
            }

            Debug.Log($"Unloading scene {gameScene.gameObject.scene.name}.");
            gameScene.OnBeforeUnload(data);
            SceneManager.UnloadSceneAsync(gameScene.gameObject.scene);
        }

        public static void ActivateGameScene(GameScene gameScene, object data)
        {
            if (gameScene == null)
            {
                throw new ArgumentNullException(nameof(gameScene));
            }

            Debug.Log($"Activating scene {gameScene.gameObject.scene.name}.");
            gameScene.gameObject.SetActive(true);
            gameScene.OnAfterActivate(data);
        }

        public static void DeactivateGameScene(GameScene gameScene, object data)
        {
            if (gameScene == null)
            {
                throw new ArgumentNullException(nameof(gameScene));
            }

            Debug.Log($"Deactivating scene {gameScene.gameObject.scene.name}.");
            gameScene.OnBeforeDeactivate(data);
            gameScene.gameObject.SetActive(false);
        }

        public static GameScene GetLoadedGameScene(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.IsValid())
            {
                Debug.LogWarning($"Scene {sceneName} is not valid.");
                return null;
            }
            return GetGameSceneFromScene(scene);
        }

        private static GameScene GetGameSceneFromScene(Scene scene)
        {
            foreach (GameObject rootObject in scene.GetRootGameObjects())
            {
                GameScene gameScene = rootObject.GetComponent<GameScene>();
                if (gameScene != null)
                {
                    return gameScene;
                }
            }
            return null;
        }

        private static void SetSceneRootObjectsActive(Scene scene, bool active)
        {
            foreach (GameObject rootObject in scene.GetRootGameObjects())
            {
                rootObject.SetActive(active);
            }
        }
    }
}
