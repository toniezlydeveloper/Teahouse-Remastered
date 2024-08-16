using System;
using DG.Tweening;
using Internal.Flow.States;
using UnityEngine;

namespace Internal.Flow.UI
{
    // ReSharper disable once InconsistentNaming
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AUIPanel : MonoBehaviour
    {
        public event Action<Type> OnTransitionRequested;

        [SerializeField] private bool isInteractable;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            GetReferences();
            Disable(true);
        }

        public void Disable(bool shouldSkipAnimation = false) => _canvasGroup.DOFade(0f, shouldSkipAnimation ? 0f : 0.25f)
            .OnStart(() =>
            {
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
            }).OnComplete(DisableCallback);

        public void Enable() => _canvasGroup.DOFade(1f, 0.25f)
            .OnStart(() =>
            {
                _canvasGroup.blocksRaycasts = isInteractable;
                _canvasGroup.interactable = isInteractable;
            }).OnComplete(EnableCallback);

        protected virtual void DisableCallback()
        {
        }
        
        protected virtual void EnableCallback()
        {
        }

        protected void RequestTransition<TState>() where TState : AState =>
            OnTransitionRequested?.Invoke(typeof(TState));
        
        private void GetReferences() => _canvasGroup = GetComponent<CanvasGroup>();
    }
}