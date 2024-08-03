using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Animations
{
    public class AnimatorObserver : MonoBehaviour
    {
        private Dictionary<AnimationCallbackType, List<Action>> _callbacksByTypes = new Dictionary<AnimationCallbackType, List<Action>>();

        public void Subscribe(AnimationCallbackType callbackType, Action callback)
        {
            if (_callbacksByTypes.TryGetValue(callbackType, out List<Action> callbacks))
            {
                callbacks.Add(callback);
            }
            else
            {
                _callbacksByTypes.Add(callbackType, new List<Action> {callback});
            }
        }
        
        public void RaiseCallback(AnimationCallbackType callbackType)
        {
            if (!_callbacksByTypes.TryGetValue(callbackType, out List<Action> callbacks))
            {
                return;
            }
            
            foreach (Action callback in callbacks)
            {
                callback?.Invoke();
            }
        }
    }
}