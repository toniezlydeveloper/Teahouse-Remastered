using System;
using Saving;
using Saving.Proxies;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SavingSetupHelper : EditorWindow
    {
        [MenuItem("Tools/just Adam/Saving Setup Helper")]
        public static void Open() => CreateWindow<SavingSetupHelper>("Saving Setup Helper");

        private void OnGUI()
        {
            foreach (FileSaveType saveType in (FileSaveType[])Enum.GetValues(typeof(FileSaveType)))
            {
                foreach (PersistenceType persistenceType in (PersistenceType[])Enum.GetValues(typeof(PersistenceType)))
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    if (GUILayout.Button($"Save {persistenceType} {saveType}"))
                    {
                        SavingController.Save(persistenceType, saveType);
                    }
            
                    if (GUILayout.Button($"Load {persistenceType} {saveType}"))
                    {
                        SavingController.Load(persistenceType, saveType);
                    }
            
                    if (GUILayout.Button($"Clear {persistenceType} {saveType}"))
                    {
                        SavingController.Clear(persistenceType, saveType);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
                
                GUILayout.Space(20);
            }
            
            if (GUILayout.Button("Setup Proxies"))
            {
                Setup();
            }
        }

        private static void Setup()
        {
            foreach (ASaveProxy proxy in FindObjectsOfType<ASaveProxy>())
            {
                EditorUtility.SetDirty(AssignId(proxy));
            }
        }

        private static ASaveProxy AssignId(ASaveProxy proxy)
        {
            proxy.Id = string.IsNullOrEmpty(proxy.Id) ? Guid.NewGuid().ToString() : proxy.Id;
            return proxy;
        }
    }
}