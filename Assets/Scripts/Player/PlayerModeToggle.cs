using System;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Player
{
    [Flags]
    public enum PlayerMode
    {
        Organization = 1 << 0,
        Modification = 1 << 1
    }
    
    public interface IPlayerModeToggle : IDependency
    {
        void Toggle(PlayerMode mode);
    }

    public class PlayerModeToggle : ADependency<IPlayerModeToggle>, IPlayerModeToggle
    {
        [SerializeField] private PlayerModeProxy modeProxy;
        
        public void Toggle(PlayerMode mode) => modeProxy.Value = mode;
    }
}