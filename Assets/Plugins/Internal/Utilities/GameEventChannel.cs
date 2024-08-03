using System;
using UnityEngine;

namespace Internal.Utilities
{
    [CreateAssetMenu(menuName = "Channels/Game Event")]
    public class GameEventChannel : ScriptableObject
    {
        public event Action OnRaised;
        
        public void Raise()
        {
            OnRaised?.Invoke();
        }
    }
}