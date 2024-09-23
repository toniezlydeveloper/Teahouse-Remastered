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
        
        [MenuItem("Tools/just Adam/Start #s")]
        public static void Start()
        {
            SceneCache.CacheScene();
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
            EditorApplication.EnterPlaymode();
        }
        
        [MenuItem("Tools/just Adam/Exit #e")]
        public static void Exit() => EditorApplication.ExitPlaymode();
        
        [MenuItem("Tools/just Adam/Game View Fullscreen &g")]
        public static void ToggleMaximizeGameView() => ToggleWindow(GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor")));

        private void OnGUI()
        {
            if (GUILayout.Button("Start"))
            {
                Start();
            }
            
            if (GUILayout.Button("Exit"))
            {
                Exit();
            }
        }

        private static void ToggleWindow(EditorWindow window) => window.maximized = !window.maximized;
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