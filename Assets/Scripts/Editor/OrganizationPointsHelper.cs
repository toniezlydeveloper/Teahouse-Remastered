using Internal.Pooling;
using Organization;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class OrganizationPointsHelper : EditorWindow
    {
        private PoolItem _itemToPlace;
        private int _angle;
        
        [MenuItem("Tools/just Adam/Organization Points Helper")]
        public static void Open() => CreateWindow<OrganizationPointsHelper>("Organization Points Helper");

        private void OnGUI()
        {
            _itemToPlace = (PoolItem)EditorGUILayout.ObjectField(_itemToPlace, typeof(PoolItem), false);
            _angle = EditorGUILayout.IntField(_angle);
            
            if (GUILayout.Button("Place"))
            {
                if (TryGetOrganizationPoint(out OrganizationPoint point))
                {
                    if (point.GetComponentInChildren<Organizable>())
                    {
                        DestroyImmediate(point.GetComponentInChildren<Organizable>().gameObject);
                    }
                    
                    if (_itemToPlace != null)
                    {
                        PoolItem x = (PoolItem)PrefabUtility.InstantiatePrefab(_itemToPlace);
                        x.transform.rotation = Quaternion.Euler(0f, _angle, 0f);
                        x.transform.parent = point.transform;
                        x.transform.localPosition = Quaternion.Euler(0f, _angle, 0f) * point.transform.forward * x.GetComponent<Organizable>().ForwardOffset;
                    }
                }
            }

            if (GUILayout.Button("Enable"))
            {
                foreach (OrganizationPoint organizationPoint in FindObjectsOfType<OrganizationPoint>())
                {
                    organizationPoint.GetComponent<MeshRenderer>().enabled = true;
                }
            }

            if (GUILayout.Button("Disable"))
            {
                foreach (OrganizationPoint organizationPoint in FindObjectsOfType<OrganizationPoint>())
                {
                    organizationPoint.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        private bool TryGetOrganizationPoint(out OrganizationPoint point)
        {
            point = Selection.activeGameObject?.GetComponent<OrganizationPoint>();
            return point != null;
        }
    }
}