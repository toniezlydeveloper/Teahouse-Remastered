using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Effectors
{
    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public class ProgressTracker : AEffector<IAddInProgress>
    {
        [SerializeField] private Image processingProgressIndicator;
        
        private float _normalizedProgress = -1f;
        
        protected override bool TryEffecting(IAddInProgress progress)
        {
            if (progress == null)
            {
                return false;
            }

            bool hasChanged = _normalizedProgress != progress.NormalizedProgress;
            _normalizedProgress = progress.NormalizedProgress;

            if (hasChanged)
            {
                processingProgressIndicator.fillAmount = _normalizedProgress;
            }
            
            return hasChanged;
        }
    }
}