using System.Collections.Generic;
using System.Linq;
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
            DrawPlacingSetupFields();

            if (GUILayout.Button("Place"))
            {
                TryPlacingItems();
            }
            
            GUILayout.Space(20);

            if (GUILayout.Button("Show"))
            {
                ShowOrganizationPoints();
            }

            if (GUILayout.Button("Hide"))
            {
                DisableOrganizationPoints();
            }

            if (GUILayout.Button("Clear"))
            {
                ClearOrganizationPoints();
            }
        }

        private void TryPlacingItems()
        {
            foreach (OrganizationPoint point in GetOrganizationPoints())
            {
                if (TryGetOrganizable(point, out Organizable organizable))
                {
                    DestroyImmediate(organizable.gameObject);
                }

                TryPlacingItem(point);
            }
        }

        private void ShowOrganizationPoints()
        {
            foreach (OrganizationPoint point in FindObjectsOfType<OrganizationPoint>())
            {
                ToggleRenderer(point, true);
            }
        }

        private void DisableOrganizationPoints()
        {
            foreach (OrganizationPoint point in FindObjectsOfType<OrganizationPoint>())
            {
                ToggleRenderer(point, false);
            }
        }

        private static void ClearOrganizationPoints()
        {
            foreach (Organizable organizable in FindObjectsOfType<OrganizationPoint>().Select(point => point.GetComponentInChildren<Organizable>()).Where(organizable => organizable != null))
            {
                DestroyImmediate(organizable.gameObject);
            }
        }

        private void DrawPlacingSetupFields()
        {
            _itemToPlace = (PoolItem)EditorGUILayout.ObjectField(_itemToPlace, typeof(PoolItem), false);
            _angle = EditorGUILayout.IntField(_angle);
        }

        private OrganizationPoint[] GetOrganizationPoints()
        {
            IEnumerable<OrganizationPoint> organizationPoints = Selection.gameObjects.Select(gameObject => gameObject.GetComponentInParent<OrganizationPoint>());
            return organizationPoints.ToArray();
        }

        private static bool TryGetOrganizable(OrganizationPoint point, out Organizable organizable)
        {
            organizable = point.GetComponentInChildren<Organizable>();
            return organizable != null;
        }

        private void TryPlacingItem(OrganizationPoint point)
        {
            if (_itemToPlace == null)
            {
                return;
            }
            
            PoolItem placedItem = (PoolItem)PrefabUtility.InstantiatePrefab(_itemToPlace);
            placedItem.transform.rotation = Quaternion.Euler(0f, _angle, 0f);
            placedItem.transform.parent = point.transform;
            placedItem.transform.localPosition = Quaternion.Euler(0f, _angle, 0f) * point.transform.forward * placedItem.GetComponent<Organizable>().ForwardOffset;
        }

        private void ToggleRenderer(OrganizationPoint point, bool state)
        {
            Transform handle = point.transform.Find("Handle");
            Renderer renderer = handle.GetComponentInChildren<MeshRenderer>();
            renderer.enabled = state;
        }
    }
}