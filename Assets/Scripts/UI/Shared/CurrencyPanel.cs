using DG.Tweening;
using Internal.Dependencies.Core;
using TMPro;
using UnityEngine;

namespace UI.Shared
{
    public interface ICurrencyPanel : IDependency
    {
        void Present(int amount);
    }
    
    public class CurrencyPanel : MonoBehaviour, ICurrencyPanel
    {
        [SerializeField] private TextMeshProUGUI currencyAmountContainer;
        
        private Sequence _currencySequence;

        private void Start() => Toggle(false);

        public void Present(int amount)
        {
            _currencySequence?.Kill();
            _currencySequence = DOTween.Sequence();
            _currencySequence.AppendCallback(() => gameObject.SetActive(true));
            _currencySequence.Append(DOTween.To(() => int.Parse(currencyAmountContainer.text), value => currencyAmountContainer.text = value.ToString(), amount, 1f).SetEase(Ease.InSine));
            _currencySequence.AppendInterval(0.5f);
            _currencySequence.AppendCallback(() => gameObject.SetActive(false));
        }
        
        private void Toggle(bool state) => gameObject.SetActive(state);
    }
}