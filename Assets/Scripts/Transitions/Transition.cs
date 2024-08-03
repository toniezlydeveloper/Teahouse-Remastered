using UnityEngine;

namespace Transitions
{
    public enum TransitionType
    {
        None,
        OpenCloseShop,
        Shop,
        Garden,
        Bedroom
    }

    public class Transition : MonoBehaviour
    {
        [SerializeField] private TransitionType type;

        private static TransitionType _targetTransitionType;

        public void Setup() => _targetTransitionType = type;
        
        public static bool ShouldToggle(TransitionType type)
        {
            if (_targetTransitionType != type)
                return false;

            _targetTransitionType = TransitionType.None;
            return true;
        }
    }
}