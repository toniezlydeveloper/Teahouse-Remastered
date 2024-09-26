using System.IO;
using System.Linq;
using Furniture;
using Grids;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class FurnitureSetupWindow : EditorWindow
    {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private GameObject[] models;
        [SerializeField] private Sprite[] icons;

        private PurchasableItemsCategoryConfig _config;
        private SerializedObject _serializedObject;
        private FurnitureCategory _category;
        private Vector3 _localRotation;
        private Vector3 _localScale;
        private GameObject _prefab;
        private Vector2 _scroll;
        private int _height;
        private int _width;

        private const string OutputPath = "Assets/Output/{0}.prefab";
        
        [MenuItem("Tools/just Adam/Furniture Setup Window")]
        public static void Open() => CreateWindow<FurnitureSetupWindow>("Furniture Setup Window");

        private void OnEnable() => GetReferences();

        private void OnGUI()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            
            DrawConversionSetupFields();

            if (GUILayout.Button("Convert To Prefabs"))
            {
                ConvertToPrefabs();
            }
            
            DrawOverrideSetupFields();

            if (GUILayout.Button("Override Config"))
            {
                OverrideConfig();
            }

            DrawOverrideIconsFields();

            if (GUILayout.Button("Override Icons"))
            {
                OverrideIcons();
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void ConvertToPrefabs()
        {
            Directory.CreateDirectory("Assets/Output");
            
            foreach (GameObject model in models)
            {
                CreatePrefabVariant(model);
            }
        }

        private void OverrideConfig()
        {
            ClearConfig();
            
            foreach (GameObject prefab in prefabs)
            {
                AddToConfig(prefab);
            }
            
            EditorUtility.SetDirty(_config);
        }

        private void OverrideIcons()
        {
            foreach (Sprite icon in icons)
            {
                OverrideIcon(icon);
                RenameIcon(icon);
            }
            
            EditorUtility.SetDirty(_config);
        }
        
        private void CreatePrefabVariant(GameObject modelPrefab)
        {
            GameObject modelInstance = (GameObject)PrefabUtility.InstantiatePrefab(modelPrefab);
            GameObject originalInstance = GetInstance(modelPrefab);
            Attach(modelInstance, originalInstance);
            SavePrefab(originalInstance);
            DestroyImmediate(originalInstance);
        }

        private GameObject GetInstance(GameObject modelPrefab)
        {
            GameObject originalInstance = (GameObject)PrefabUtility.InstantiatePrefab(_prefab);
            originalInstance.name = $"P_{modelPrefab.name.Replace("_", "")}";
            return originalInstance;
        }

        private void Attach(GameObject modelInstance, GameObject originalInstance)
        {
            modelInstance.transform.SetParent(originalInstance.transform);
            modelInstance.transform.localRotation = Quaternion.Euler(_localRotation);
            modelInstance.transform.localScale = _localScale;
        }

        private void SavePrefab(GameObject originalInstance)
        {
            string prefabPath = string.Format(OutputPath, originalInstance);
            PrefabUtility.SaveAsPrefabAsset(originalInstance, prefabPath);
        }

        private void ClearConfig() => _config.Set.Clear();
        
        private void AddToConfig(GameObject prefab)
        {
            _config.Set.Add(new FurniturePiece
            {
                Category = _category,
                Dimensions = new GridDimensions { Height = _height, Width = _width },
                Name = prefab.name.Replace("P_", "").Replace("_", ""),
                Prefab = prefab
            });
        }
        
        private void OverrideIcon(Sprite icon)
        {
            FurniturePiece matchingPiece = _config.Set.FirstOrDefault(piece => icon.name.EndsWith(piece.Name));

            if (matchingPiece == null)
            {
                return;
            }

            matchingPiece.Icon = icon;
        }

        private void RenameIcon(Sprite icon)
        {
            string newName = Path.GetFileName(AssetDatabase.GetAssetPath(icon.texture)).Replace("P_", "T_");
            string oldPath = AssetDatabase.GetAssetPath(icon.texture);
            AssetDatabase.RenameAsset(oldPath, newName);
            EditorUtility.SetDirty(icon.texture);
        }

        private void DrawConversionSetupFields()
        {
            _prefab = (GameObject)EditorGUILayout.ObjectField("Original Prefab", _prefab, typeof(GameObject), false);
            _localRotation = EditorGUILayout.Vector3Field("Local Rotation", _localRotation);
            _localScale = EditorGUILayout.Vector3Field("Local Scale", _localScale);
            EditorGUILayout.PropertyField(_serializedObject.FindProperty(nameof(models)));
            _serializedObject.ApplyModifiedProperties();
        }

        private void DrawOverrideSetupFields()
        {
            GUILayout.Space(20);
            _config = (PurchasableItemsCategoryConfig)EditorGUILayout.ObjectField("Output Config", _config, typeof(PurchasableItemsCategoryConfig), false);
            _category = (FurnitureCategory)EditorGUILayout.EnumPopup("Models Category", _category);
            _height = EditorGUILayout.IntField("Model Height", _height);
            _width = EditorGUILayout.IntField("Model Width", _width);
            EditorGUILayout.PropertyField(_serializedObject.FindProperty(nameof(prefabs)));
            _serializedObject.ApplyModifiedProperties();
        }

        private void DrawOverrideIconsFields()
        {
            GUILayout.Space(20);
            _config = (PurchasableItemsCategoryConfig)EditorGUILayout.ObjectField("Output Config", _config, typeof(PurchasableItemsCategoryConfig), false);
            EditorGUILayout.PropertyField(_serializedObject.FindProperty(nameof(icons)));
            _serializedObject.ApplyModifiedProperties();
        }

        private void GetReferences() => _serializedObject = new SerializedObject(this);
    }
}