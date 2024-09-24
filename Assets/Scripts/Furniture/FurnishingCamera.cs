using System;
using Cinemachine;
using Internal.Dependencies.Core;
using States;
using UnityEngine;

namespace Furniture
{
    public class FurnishingCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
        
        private IFurnishingCore _furnishingCore = DependencyInjector.Get<IFurnishingCore>();

        private void Update() => cinemachineCamera.Priority = _furnishingCore.IsEnabled ? 15 : 0;
    }
}