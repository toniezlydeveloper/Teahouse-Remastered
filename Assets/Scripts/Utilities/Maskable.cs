using UnityEngine;

namespace Utilities
{
    public class Maskable : MonoBehaviour
    {
        [SerializeField] private Renderer maskableRenderer;

        private const int MaskedRenderQueue = 3000;
        
        private void Start() => maskableRenderer.material.renderQueue = MaskedRenderQueue;
    }
}