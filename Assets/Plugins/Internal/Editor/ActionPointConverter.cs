using Internal.Sequences;
using UnityEditor;
using UnityEngine;

namespace Internal.Editor
{
    public class ActionPointConverter : EditorWindow
    {
        private Transform _point;
        
        [MenuItem("Tools/just Adam/Action Point Converter")]
        public static void Open() => GetWindow<ActionPointConverter>("Action Point Converter");

        private void OnGUI()
        {
            if (LocalGUIUtilities.Button("Convert To Point", TryGetPoint))
            {
                ConvertToPoint();
            }
        }

        private void ConvertToPoint() => AssetDatabase.CreateAsset(GetPoint(out Vector3 point), GetOutputPath(point));

        private bool TryGetPoint()
        {
            GameObject pointGameObject = GameObject.Find("ActionPoint");

            if (pointGameObject == null)
            {
                return false;
            }

            _point = pointGameObject.transform;
            return true;
        }

        private SequencePoint GetPoint(out Vector3 viewportPoint)
        {
            viewportPoint = Camera.main!.WorldToViewportPoint(_point.position);
            SequencePoint point = CreateInstance<SequencePoint>();
            point.ViewportPoint = viewportPoint;
            return point;
        }

        private static string GetOutputPath(Vector3 point) => $"Assets/Settings/Points/{point.x:0.00} {point.y:0.00}.asset";
    }
}