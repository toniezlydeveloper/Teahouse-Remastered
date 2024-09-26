using UnityEngine;

namespace Utilities
{
    [ExecuteInEditMode]
    public class SizeHelper : MonoBehaviour
    {
        [SerializeField] private Vector3 size;

        private Renderer _renderer;
        
        #if UNITY_EDITOR

        private void Awake() => _renderer = GetComponent<Renderer>();
        
        private void Update() => size = _renderer.bounds.size;
        
        #endif
    }
}