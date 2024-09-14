using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class GeneralHelper : EditorWindow
    {
        [MenuItem("Tools/just Adam/General Helper")]
        public static void Open() => CreateWindow<GeneralHelper>("General Helper");

        private void OnGUI()
        {
            if (GUILayout.Button("Start"))
            {
                SceneCache.CacheScene();
                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
                EditorApplication.EnterPlaymode();
            }
            
            if (GUILayout.Button("Stop"))
            {
                EditorApplication.ExitPlaymode();
            }
        }
    }

    [InitializeOnLoad]
    public static class SceneCache
    {
        private const string SceneNameKey = "PreviouslyOpenedScene";
        
        static SceneCache() => EditorApplication.playModeStateChanged += TryRestoringScene;

        public static void CacheScene() => EditorPrefs.SetString(SceneNameKey, GetPath(SceneManager.GetActiveScene()));
        
        private static void TryRestoringScene(PlayModeStateChange change)
        {
            if (!HasExitedPlayMode(change))
            {
                return;
            }

            if (!EditorPrefs.HasKey(SceneNameKey))
            {
                return;
            }
            
            EditorSceneManager.OpenScene(EditorPrefs.GetString(SceneNameKey));
        }

        private static bool HasExitedPlayMode(PlayModeStateChange change) => change == PlayModeStateChange.EnteredEditMode;

        private static string GetPath(Scene scene) => scene.path;
    }
}