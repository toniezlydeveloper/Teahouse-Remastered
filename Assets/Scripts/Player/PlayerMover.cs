using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private InputActionReference moveInput;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float moveSpeed;

        private Transform _mainCameraTransform;
        
        private void Start() => _mainCameraTransform = Camera.main!.transform;

        private void Update()
        {
            Vector3 direction = ReadWorldDirectionFromInput();
            Rotate(direction);
            Move(direction);
        }

        private void Rotate(Vector3 direction)
        {
            if (direction.sqrMagnitude <= 0.005f)
                return;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }

        private void Move(Vector3 direction) => characterController.Move(direction * (moveSpeed * Time.deltaTime) + Physics.gravity);

        private Vector3 ReadWorldDirectionFromInput()
        {
            Vector2 input = moveInput.action.ReadValue<Vector2>();
            Vector3 forward = Vector3.ProjectOnPlane(_mainCameraTransform.forward, Vector3.up);
            Vector3 right = Vector3.ProjectOnPlane(_mainCameraTransform.right, Vector3.up);
            return Vector3.ClampMagnitude(forward * input.y + right * input.x, 1f);
        }
    }
}