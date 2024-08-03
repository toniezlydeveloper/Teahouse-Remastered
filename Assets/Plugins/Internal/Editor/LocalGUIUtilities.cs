using System;
using UnityEngine;

namespace Internal.Editor
{
    public static class LocalGUIUtilities
    {
        public static bool Button(string text, Func<bool> enableCallback = null)
        {
            GUI.enabled = enableCallback == null || enableCallback.Invoke();
            bool clickedButton = GUILayout.Button(text);
            GUI.enabled = true;
            return clickedButton;
        }
    }
}