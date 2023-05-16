using RPGMaker.Codebase.Runtime.Addon;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace RPGMaker.Codebase.Addon
{
    class LoadAddonSceneEditor
    {
        private static string _addonName = "LoadAddonScene";
        private static string _scenePath = "Assets/RPGMaker/Codebase/Add-ons/LoadAddonScene/AddonScene.unity";

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad() {
            EditorApplication.playModeStateChanged += (playModeStateChange) =>
            {
                if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
                {
                    Initialize();
                }
            };
            Initialize();
        }

        private static async Task Initialize() {
            await Task.Delay(10);

            foreach (var info in AddonManager.Instance.GetAddonRuntimeInfos())
            {
                if (info.addonName == _addonName)
                {
                    var method = info.instance.GetType().GetMethod("Enabled", BindingFlags.Public | BindingFlags.Instance);
                    if (method != null)
                    {
                        var obj = method.Invoke(info.instance, null);
                        if (obj is bool && (bool) obj)
                        {
                            await Task.Delay(500);
                            OnInitialize();
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// unity起動時、プレイモード終了時に呼び出される処理
        /// Processes called at Unity startup and when exiting play mode.
        /// </summary>
        private static void OnInitialize() {
            CheckAndAddScene(_scenePath);
        }

        /// <summary>
        /// このコードは指定したシーンがビルド設定に存在しない場合にのみシーンを追加します。また、シーンを追加する前にユーザーに確認を求めるダイアログを表示します。
        /// This code adds the specified scene to the build settings only if it does not already exist there. Also, before adding the scene, it displays a dialog asking the user for confirmation.
        /// </summary>
        /// <param name="scenePath">scenePath</param>
        public static void CheckAndAddScene(string scenePath) {
            // Get the current list of scenes in the build settings
            var scenes = EditorBuildSettings.scenes;

            // Check if the scene is already in the build settings
            if (scenes.Any(scene => scene.path == scenePath))
            {
                Debug.Log("Scene is already in the build settings.");
                return;
            }

            // Ask the user if they want to add the scene
            if (EditorUtility.DisplayDialog("Scene not in build settings",
                                            $"The scene at {scenePath} is not in the build settings. Would you like to add it?",
                                            "Yes",
                                            "No"))
            {
                // Add the scene to the build settings
                var newScene = new EditorBuildSettingsScene(scenePath, true);
                var newScenes = scenes.ToList();
                newScenes.Add(newScene);
                EditorBuildSettings.scenes = newScenes.ToArray();

                Debug.Log("Scene added to build settings.");
            }
        }
    }
}
