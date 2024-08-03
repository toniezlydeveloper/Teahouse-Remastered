using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Effectors
{
    public class CupCleaner : AEffector<Cup>
    {
        [SerializeField] private Image cleaningProgressIndicator;
        [SerializeField] private float cleaningDuration;
        
        private float _cleaningTime;

        protected override void Cache(object _)
        {
            SetupCleaningTime();
            base.Cache(_);
        }

        protected override bool TryEffecting(Cup cup)
        {
            cleaningProgressIndicator.fillAmount = 0f;

            if (cup == null)
                return false;
            
            if (!cup.IsDirty)
                return false;
            
            cleaningProgressIndicator.fillAmount = Mathf.Clamp01((_cleaningTime - Time.time) / cleaningDuration);
            
            if (Time.time < _cleaningTime)
                return false;

            cup.IsDirty = false;
            return true;
        }

        private void SetupCleaningTime() => _cleaningTime = Time.time + cleaningDuration;
    }
}