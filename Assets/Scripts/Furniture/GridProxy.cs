using Grids;
using Internal.Dependencies.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furniture
{
    public class GridProxy : ADependency<IGridPointer>, IGridPointer
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform pointer;

        private Camera _mainCamera;
        
        public Vector3 Position => pointer.position;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Vector2 x = Pointer.current.position.ReadValue();
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(x), out RaycastHit hit, 100f, groundLayer))
            {
                pointer.position = hit.point;
            }
            else
            {
                pointer.position = Vector3.right * 1000f;
            }
                
        }
    }
}