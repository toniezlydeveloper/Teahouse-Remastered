using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Effectors
{
    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public class ProcessingProgressTracker : AEffector<IAddInProgress>
    {
        [SerializeField] private Image processingProgressIndicator;
        
        private float _normalizedProgress = -1f;
        
        protected override bool TryEffecting(IAddInProgress progress)
        {
            float value = progress?.NormalizedProgress ?? 0f;
            bool hasChanged = _normalizedProgress != value;
            _normalizedProgress = value;

            if (hasChanged)
            {
                processingProgressIndicator.fillAmount = _normalizedProgress;
            }
            
            return hasChanged;
        }
    }
}