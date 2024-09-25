using Cinemachine;
using Internal.Dependencies.Core;
using States;
using UnityEngine;

namespace Furniture
{
    public class FurnishingCamera : ADependencyElement<IFurnishingListener>, IFurnishingListener
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
        
        public void Toggle(bool state) => cinemachineCamera.Priority = state ? 15 : 0;
    }
}