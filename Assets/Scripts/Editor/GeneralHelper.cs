using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class GeneralHelper : EditorWindow
    {
        private string _previousScenePath;
        
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
        private static string _previousScenePath;

        static SceneCache() => EditorApplication.playModeStateChanged += TryRestoringScene;

        public static void CacheScene() => CacheScene(SceneManager.GetActiveScene());
        
        private static void TryRestoringScene(PlayModeStateChange change)
        {
            if (!HasExitedPlayMode(change))
            {
                return;
            }
            
            EditorSceneManager.OpenScene(GetCachedScenePath());
        }

        private static bool HasExitedPlayMode(PlayModeStateChange change) => change == PlayModeStateChange.EnteredEditMode;

        private static void CacheScene(Scene scene) => _previousScenePath = scene.path;

        private static string GetCachedScenePath() => _previousScenePath;
    }
}