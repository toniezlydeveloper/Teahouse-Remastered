using Internal.Dependencies.Core;
using States;
using UnityEngine;

namespace Player
{
    public class PlayerPositioner : ADependencyElement<IFurnishingListener>, IFurnishingListener
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform point;
        
        public void Toggle(bool state)
        {
            TogglePlayer(state);
            
            if (!ShouldTeleport(state))
            {
                return;
            }
            
            Teleport();
        }

        private static bool ShouldTeleport(bool state) => state;

        private void TogglePlayer(bool state) => player.gameObject.SetActive(!state);

        private void Teleport()
        {
            player.position = point.position;
            player.forward = point.forward;
        }
    }
}