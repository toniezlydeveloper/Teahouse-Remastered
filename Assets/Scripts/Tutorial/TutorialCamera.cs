using Cinemachine;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Tutorial
{
    public interface ITutorialCamera : IDependency
    {
        void Toggle(TutorialCameraType type);
    }

    public class TutorialCamera : ADependencyElement<ITutorialCamera>, ITutorialCamera
    {
        [SerializeField] private TutorialCameraType enabledType;

        private ICinemachineCamera _camera;
        
        private const int DisabledPriority = 0;
        private const int EnabledPriority = 10;

        private void Start() => _camera = GetComponent<ICinemachineCamera>();

        public void Toggle(TutorialCameraType type)
        {
            _camera.Priority = type == enabledType ? EnabledPriority : DisabledPriority;
        }
    }
}