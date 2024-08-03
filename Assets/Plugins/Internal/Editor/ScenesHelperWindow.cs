using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Internal.Editor
{
    public class ScenesHelperWindow : EditorWindow
    {
        private GUIStyle _evenBackgroundStyle;
        private GUIStyle _oddBackgroundStyle;
        
        private List<string> _buildScenePaths;
        private List<string> _otherScenePaths;
        private Vector2 _scrollPosition;

        private const string LoadSingleText = "Load Single";
        private const string UnloadText = "Unload";
        private const string PingText = "Ping";
        private const string LoadAdditiveText = "Load Additive";
        private const string BuildScenesText = "In Build";
        private const string OtherScenesText = "Other";
        private const string RefreshText = "Refresh";
        private const int Spacing = 20;

        [MenuItem("Tools/just Adam/Scenes Helper")]
        public static void Open() => CreateWindow<ScenesHelperWindow>("Scenes Helper");

        private void OnEnable()
        {
            GetScenePaths();
            GetStyles();
        }

        private void OnGUI()
        {
            BeginScenesView();
            DrawBuildScenes();
            DrawOtherScenes();
            EnsScenesView();
            DrawRefreshFields();
        }

        private void BeginScenesView()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical();
        }

        private static void EnsScenesView()
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void DrawBuildScenes()
        {
            EditorGUILayout.LabelField(BuildScenesText, EditorStyles.boldLabel);

            for (int i = 0; i < _buildScenePaths.Count; i++)
            {
                DrawSceneFields(_buildScenePaths[i], i);
            }
            
            GUILayout.Space(Spacing);
        }

        private void DrawOtherScenes()
        {
            EditorGUILayout.LabelField(OtherScenesText, EditorStyles.boldLabel);

            for (int i = 0; i < _otherScenePaths.Count; i++)
            {
                DrawSceneFields(_otherScenePaths[i], i);
            }
        }

        private void DrawSceneFields(string scenePath, int index)
        {
            EditorGUILayout.BeginHorizontal(BackgroundStyleByIndex(index));
            EditorGUILayout.LabelField(Path.GetFileNameWithoutExtension(scenePath));

            if (GUILayout.Button(LoadSingleText))
            {
                EditorSceneManager.OpenScene(scenePath);
            }

            if (GUILayout.Button(LoadAdditiveText))
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }

            if (GUILayout.Button(UnloadText))
            {
                EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(scenePath), true);
            }

            if (GUILayout.Button(PingText))
            {
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(scenePath));
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRefreshFields()
        {
            if (!GUILayout.Button(RefreshText))
            {
                return;
            }

            GetScenePaths();
        }

        private GUIStyle BackgroundStyleByIndex(int index) => index % 2 == 0 ? _evenBackgroundStyle : _oddBackgroundStyle;

        private void GetScenePaths()
        {
            _buildScenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            _otherScenePaths = AssetDatabase.FindAssets("t:scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => _buildScenePaths.All(buildScenePath => buildScenePath != path))
                .ToList();
        }

        private void GetStyles()
        {
            _evenBackgroundStyle = new GUIStyle
            {
                normal =
                {
                    background = Texture2D.grayTexture
                }
            };

            _oddBackgroundStyle = new GUIStyle();
        }
    }
}