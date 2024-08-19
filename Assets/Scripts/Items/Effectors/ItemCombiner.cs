using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Effectors
{
    public class ItemCombiner : AEffector<CombinationItem>
    {
        [SerializeField] private Image combinationProgressIndicator;
        [SerializeField] private float combinationDuration;
        
        private float _combinationTime;

        protected override void Cache(object _)
        {
            SetupCombinationTime();
            base.Cache(_);
        }

        protected override bool TryEffecting(CombinationItem item)
        {
            combinationProgressIndicator.fillAmount = 0f;

            if (item == null)
                return false;
            
            if (item.IsCombined)
                return false;

            if (item.Item1 == null)
                return false;

            if (item.Item2 == null)
                return false;
            
            combinationProgressIndicator.fillAmount = 1f - Mathf.Clamp01((_combinationTime - Time.time) / combinationDuration);
            
            if (Time.time < _combinationTime)
                return false;

            // todo: implement
            // item.Output = COMBINED ITEM;
            item.IsCombined = true;
            return true;
        }

        private void SetupCombinationTime() => _combinationTime = Time.time + combinationDuration;
    }
}