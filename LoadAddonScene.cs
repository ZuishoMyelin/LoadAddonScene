using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*:en
 * @addondesc AddonScene additional loading when transitioning to SceneMap
 * @author Zuisho
*/

/*:ja
 * @addondesc SceneMapへの遷移時にAddonSceneを追加ロードする
 * @author Zuisho
 */

namespace RPGMaker.Codebase.Addon
{
    public class LoadAddonScene
    {
        private static string _addonSceneName = "AddonScene";

        private bool _enabled = false;

        public bool Enabled() {
            return _enabled;
        }

        public LoadAddonScene() {
            _enabled = true;
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    AddonRunner();
                }
            };
#else
            AddonRunner();
#endif
        }

        private static void AddonRunner() {
            SceneManager.sceneLoaded += (scene, LoadSceneMode) =>
            {
                if (SceneManager.GetActiveScene().name == "SceneMap" && !ContainsScene(_addonSceneName))
                {
                    SceneManager.LoadSceneAsync(_addonSceneName, LoadSceneMode.Additive);
                }
            };
        }

        private static bool ContainsScene(string sceneName) {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}