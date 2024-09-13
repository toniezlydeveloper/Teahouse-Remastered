using System;
using UnityEditor;
using UnityEngine;

namespace Saving.Items
{
    public class SavingSetupHelper : EditorWindow
    {
        [MenuItem("Tools/just Adam/Saving Setup Helper")]
        public static void Open() => CreateWindow<SavingSetupHelper>("Saving Setup Helper");

        private void OnGUI()
        {
            if (GUILayout.Button("Setup Proxies"))
            {
                Setup();
            }
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Save Shop"))
            {
                SavingController.Save(SaveType.Shop);
            }
            
            if (GUILayout.Button("Load Shop"))
            {
                SavingController.Load(SaveType.Shop);
            }
            
            if (GUILayout.Button("Clear Shop"))
            {
                SavingController.Clear(SaveType.Shop);
            }
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Save Bedroom"))
            {
                SavingController.Save(SaveType.Bedroom);
            }
            
            if (GUILayout.Button("Load Bedroom"))
            {
                SavingController.Load(SaveType.Bedroom);
            }
            
            if (GUILayout.Button("Clear Bedroom"))
            {
                SavingController.Clear(SaveType.Bedroom);
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