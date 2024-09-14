using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minZ;
        [SerializeField] private float maxZ;
        
        private PlayerMover _player;
        
        private void Start()
        {
            _player = FindObjectOfType<PlayerMover>();
        }

        private void Update()
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(_player.transform.position.x, minX, maxX);
            position.z = Mathf.Clamp(_player.transform.position.z, minZ, maxZ);
            transform.position = position;
        }
    }
}