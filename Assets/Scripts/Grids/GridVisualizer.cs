using Player;
using UnityEngine;

namespace Grids
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private PlayerModeProxy playerMode;
        [SerializeField] private Renderer grid;
        
        private static readonly int VisibilityId = Shader.PropertyToID("_Alpha");

        private void Start() => playerMode.OnChanged += Toggle;

        private void OnDestroy() => playerMode.OnChanged -= Toggle;

        private void Toggle(PlayerMode mode) => grid.material.SetFloat(VisibilityId, mode == PlayerMode.Organization ? 1f : 0f);
    }
}