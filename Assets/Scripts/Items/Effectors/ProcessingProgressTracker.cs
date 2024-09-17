using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Effectors
{
    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public class ProcessingProgressTracker : AEffector<IAddInProgress>
    {
        [SerializeField] private Image processingProgressIndicator;
        [SerializeField] private EffectorsConfig config;
        
        private float _normalizedProgress = -1f;
        
        protected override bool TryEffecting(IAddInProgress progress)
        {
            float value = progress?.NormalizedProgress ?? 0f;
            bool hasChanged = _normalizedProgress != value;
            _normalizedProgress = value;

            if (!hasChanged)
            {
                return false;
            }
            
            processingProgressIndicator.color = value < 1f ? config.ProcessingColor : config.ProcessedColor;
            processingProgressIndicator.fillAmount = _normalizedProgress;
            return true;
        }
    }
}