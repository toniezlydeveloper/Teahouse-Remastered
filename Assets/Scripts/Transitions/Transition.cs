using UnityEngine;

namespace Transitions
{
    public enum TransitionType
    {
        None = 0,
        OpenCloseShop = 1,
        Shop = 2,
        Bedroom = 4,
        ItemShop = 5
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