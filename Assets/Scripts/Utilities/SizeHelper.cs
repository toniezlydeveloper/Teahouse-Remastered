using UnityEngine;

namespace Utilities
{
    [ExecuteInEditMode]
    public class SizeHelper : MonoBehaviour
    {
        [SerializeField] private Vector3 size;
        
        #if UNITY_EDITOR
        private void Update() => size = GetComponent<Renderer>().bounds.size;
        #endif
    }
}