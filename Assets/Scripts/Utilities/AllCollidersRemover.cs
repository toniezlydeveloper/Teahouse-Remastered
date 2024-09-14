using UnityEngine;

namespace Utilities
{
    public class AllCollidersRemover : MonoBehaviour
    {
        private void Reset()
        {
            foreach (Collider childCollider in GetComponentsInChildren<Collider>())
            {
                DestroyImmediate(childCollider);
            }
        }
    }
}