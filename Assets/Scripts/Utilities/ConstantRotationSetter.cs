using UnityEngine;

namespace Utilities
{
    public class ConstantRotationSetter : MonoBehaviour
    {
        [SerializeField] private Vector3 lookRotation;

        private void Update() => transform.forward = lookRotation;
    }
}
