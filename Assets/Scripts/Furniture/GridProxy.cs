using Grids;
using Internal.Dependencies.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Furniture
{
    public class GridProxy : ADependency<IGridPointer>, IGridPointer
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform pointer;

        private Camera _mainCamera;
        
        public Vector3 Position => pointer.position;

        private static readonly Vector3 DefaultPointerPosition = Vector3.right * 1000f;

        private const float RaycastDistance = 100f;

        private void Start() => _mainCamera = Camera.main;

        private void Update()
        {
            Vector2 pointerPosition = Pointer.current.position.ReadValue();
            
            if (UIHelpers.IsPointerOverUI() || !Physics.Raycast(_mainCamera.ScreenPointToRay(pointerPosition), out RaycastHit hit, RaycastDistance, groundLayer))
            {
                pointer.position = DefaultPointerPosition;
            }
            else
            {
                pointer.position = hit.point;
            }
        }
    }
}