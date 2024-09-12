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
            if (GUILayout.Button("Setup"))
            {
                foreach (ASaveProxy item in FindObjectsOfType<ASaveProxy>())
                {
                    item.Id = Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
            
            if (GUILayout.Button("Save"))
            {
                SavingController.Save();
            }
            
            if (GUILayout.Button("Load"))
            {
                SavingController.Load();
            }
        }
    }
}