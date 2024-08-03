using UnityEngine;
using UnityEngine.Events;

namespace Internal.Utilities
{
    public class GameEventObserver : MonoBehaviour
    {
        [SerializeField] private GameEventChannel gameEventChannel;
        [SerializeField] private UnityEvent callback;

        private void Awake()
        {
            gameEventChannel.OnRaised += RaiseCallback;
        }

        private void OnDestroy()
        {
            gameEventChannel.OnRaised -= RaiseCallback;
        }

        private void RaiseCallback()
        {
            callback?.Invoke();
        }
    }
}