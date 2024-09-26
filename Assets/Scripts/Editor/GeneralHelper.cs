using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public static class GeneralHelper
    {
        private const string SceneNameKey = "PreviouslyOpenedScene";
        
        static GeneralHelper() => EditorApplication.playModeStateChanged += TryRestoringScene;
        
        [MenuItem("Tools/just Adam/Toggle #s")]
        public static void Toggle()
        {
            if (IsInIdle())
            {
                CacheScene();
                LoadDefaultBuildScene();
                Start();
            }
            else
            {
                Stop();
            }
            
        }
        
        [MenuItem("Tools/just Adam/Game View Fullscreen &g")]
        public static void ToggleMaximizeGameView() => ToggleWindow(EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor")));
        
        [MenuItem("Tools/just Adam/Load Previous Build Scene #e")]
        public static void LoadPreviousBuildScene() => EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(GetBuildIndex(SceneManager.GetActiveScene(), -1)));

        
        [MenuItem("Tools/just Adam/Load Next Build Scene #q")]
        public static void LoadNextBuildScene() => EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(GetBuildIndex(SceneManager.GetActiveScene(), 1)));

        
        [MenuItem("Tools/just Adam/Load Default Build Scene #w")]
        public static void LoadDefaultBuildScene() => EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));

        private static void CacheScene() => EditorPrefs.SetString(SceneNameKey, GetPath(SceneManager.GetActiveScene()));

        private static void Start() => EditorApplication.EnterPlaymode();
        
        private static void Stop() => EditorApplication.ExitPlaymode();
        
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

        private static bool IsInIdle() => !EditorApplication.isPlaying;

        private static void ToggleWindow(EditorWindow window) => window.maximized = !window.maximized;

        private static int GetBuildIndex(Scene scene, int delta)
        {
            int count = SceneManager.sceneCountInBuildSettings;
            return (scene.buildIndex + delta + count) % count;
        }
    }
}