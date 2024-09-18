using UnityEngine;

namespace Organization
{
    public class Organizable : MonoBehaviour
    {
        [field: SerializeField] public float ForwardOffset { get; set; }
        
        public string Name => gameObject.name.Replace("(Clone)", "");
        public Quaternion Rotation => transform.localRotation;
        public GameObject GameObject => gameObject;
    }
}