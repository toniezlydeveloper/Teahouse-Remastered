using System;
using System.Linq;
using Bedroom;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerCamera
    {
        [field: SerializeField] public GameObject Camera { get; set; }
        [field: SerializeField] public PlayerMode Mode { get; set; }
        [field: SerializeField] public DayTime Time { get; set; }
        [field: SerializeField] public float MinX { get; set; }
        [field: SerializeField] public float MaxX { get; set; }
        [field: SerializeField] public float MinZ { get; set; }
        [field: SerializeField] public float MaxZ { get; set; }
    }
    
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private PlayerCamera[] playerCameras;
        [SerializeField] private PlayerModeProxy playerMode;
        [SerializeField] private DayTimeProxy dayTime;

        private PlayerCamera _playerCamera;
        private PlayerMover _player;
        
        private void Start()
        {
            GetReferences();
            AddCallbacks();
            Refresh();
        }

        private void OnDestroy() => RemoveCallbacks();

        private void Update() => FollowPlayer();

        private void Refresh(PlayerMode _) => Refresh();

        private void Refresh(DayTime _) => Refresh();

        private void Refresh()
        {
            _playerCamera = playerCameras.First(playerCamera => playerCamera.Time.HasFlag(dayTime.Value) && playerCamera.Mode.HasFlag(playerMode.Value));
            
            foreach (PlayerCamera playerCamera in playerCameras)
            {
                playerCamera.Camera.SetActive(false);
            }
            
            _playerCamera.Camera.SetActive(true);
        }

        private void FollowPlayer()
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(_player.transform.position.x, _playerCamera.MinX, _playerCamera.MaxX);
            position.z = Mathf.Clamp(_player.transform.position.z, _playerCamera.MinZ, _playerCamera.MaxZ);
            transform.position = position;
        }

        private void GetReferences() => _player = FindObjectOfType<PlayerMover>();

        private void AddCallbacks()
        {
            playerMode.OnChanged += Refresh;
            dayTime.OnChanged += Refresh;
        }

        private void RemoveCallbacks()
        {
            playerMode.OnChanged -= Refresh;
            dayTime.OnChanged -= Refresh;
        }
    }
}