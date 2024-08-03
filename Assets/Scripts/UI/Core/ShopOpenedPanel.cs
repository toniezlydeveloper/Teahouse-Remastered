using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Interaction;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using Items.Holders;
using Player;
using TMPro;
using UI.ItemPanels;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Core
{
    public interface ITimePanel : IDependency
    {
        void Present(float[] customerSpawnNormalizedTimes);
        void Present(float normalizedTime);
        void Present();
    }

    public interface ICurrencyPanel : IDependency
    {
        void Present(int amount);
    }

    public class ShopOpenedPanel : AUIPanel, IItemPanel, ITimePanel, ICurrencyPanel
    {
        [Header("Time Panel")]
        [SerializeField] private GameObject customerMarkerPrefab;
        [SerializeField] private GameObject timePanel;
        [SerializeField] private Image timeBar;

        [Header("Currency Panel")]
        [SerializeField] private TextMeshProUGUI currencyAmountContainer;
        [SerializeField] private GameObject currencyPanel;

        private Dictionary<PlayerMode, ModeHints> _hintsByMode;
        private List<Image> _customerMarkers = new();
        private Sequence _currencySequence;
        private IItemPanel[] _itemPanels;

        private void Start()
        {
            GetReferences();
            ToggleTimePanel(false);
            ToggleCurrencyPanel(false);
        }

        public void Present(float[] customerSpawnNormalizedTimes)
        {
            foreach (float normalizedTime in customerSpawnNormalizedTimes)
                SpawnCustomerMarker(normalizedTime);
            
            ToggleTimePanel(true);
        }
        
        public void Present()
        {
            DestroyCustomerMarkers();
            Present(0f);
            ToggleTimePanel(false);
        }
        
        public void Present(float normalizedTime) => timeBar.fillAmount = normalizedTime;

        public void Present(IItemHolder itemHolder)
        {
            foreach (IItemPanel itemPanel in _itemPanels)
                itemPanel.Present(itemHolder);
        }

        public void Present(int amount)
        {
            _currencySequence?.Kill();
            _currencySequence = DOTween.Sequence();
            _currencySequence.AppendCallback(() => currencyPanel.SetActive(true));
            _currencySequence.Append(DOTween.To(() => int.Parse(currencyAmountContainer.text), value => currencyAmountContainer.text = value.ToString(), amount, 1f).SetEase(Ease.InSine));
            _currencySequence.AppendInterval(0.5f);
            _currencySequence.AppendCallback(() => currencyPanel.SetActive(false));
        }

        private void ToggleTimePanel(bool state) => timePanel.SetActive(state);
        
        private void ToggleCurrencyPanel(bool state) => currencyPanel.SetActive(state);

        private void SpawnCustomerMarker(float normalizedPosition)
        {
            Image customerMarker = Instantiate(customerMarkerPrefab, timeBar.transform).GetComponentInChildren<Image>();
            Vector3 markerPosition = customerMarker.rectTransform.localPosition;
            float timeBarWidth = ((RectTransform)timeBar.transform).rect.width;
            _customerMarkers.Add(customerMarker);
            
            markerPosition.x = timeBarWidth * normalizedPosition - timeBarWidth * 0.5f;
            customerMarker.rectTransform.localPosition = markerPosition;
        }

        private void DestroyCustomerMarkers()
        {
            foreach (Image customerMarker in _customerMarkers)
                Destroy(customerMarker.gameObject);

            _customerMarkers.Clear();
        }

        private void GetReferences() => _itemPanels = GetComponents<IItemPanel>().Where(panel => panel.GetType() != typeof(ShopOpenedPanel)).ToArray();
    }
}