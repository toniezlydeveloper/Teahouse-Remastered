using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private PlayerMover _player;
        
        private void Start()
        {
            _player = FindObjectOfType<PlayerMover>();
        }

        private void Update()
        {
            Vector3 position = transform.position;
            position.x = _player.transform.position.x;
            position.z = _player.transform.position.z;
            transform.position = position;
        }
    }
}