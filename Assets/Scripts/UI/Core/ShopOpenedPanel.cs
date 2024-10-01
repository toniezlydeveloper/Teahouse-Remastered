using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Interaction;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using Items.Holders;
using Player;
using UI.Helpers;
using UI.Items;
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

    public interface INotesPanel : IDependency
    {
        void Init(OrderData[] ordersData);
        void Toggle(bool state);
    }

    public class OrderData
    {
        public List<Enum> AddIns { get; set; }
        public Species Species { get; set; }
    }

    public class ShopOpenedPanel : AUIPanel, IItemPanel, ITimePanel, INotesPanel
    {
        [SerializeField] private GameObject customerMarkerPrefab;
        [SerializeField] private CustomerOrder orderPrefab;
        [SerializeField] private Transform ordersParent;
        [SerializeField] private GameObject notesPanel;
        [SerializeField] private GameObject timePanel;
        [SerializeField] private Image timeBar;

        private Dictionary<PlayerMode, ModeHints> _hintsByMode;
        private List<Image> _customerMarkers = new();
        private List<GameObject> _orders = new();
        private IItemPanel[] _itemPanels;

        private void Start()
        {
            GetReferences();
            ToggleTimePanel(false);
        }

        public void Present(float[] customerSpawnNormalizedTimes)
        {
            foreach (float normalizedTime in customerSpawnNormalizedTimes)
            {
                SpawnCustomerMarker(normalizedTime);
            }
            
            ToggleTimePanel(true);
        }
        
        public void Present()
        {
            DestroyCustomerMarkers();
            Present(0f);
            ToggleTimePanel(false);
        }
        
        public void Init(OrderData[] ordersData)
        {
            foreach (GameObject order in _orders)
            {
                Destroy(order);
            }

            foreach (OrderData data in ordersData)
            {
                Init(Instantiate(orderPrefab, ordersParent), data);
            }
        }
        
        public void Present(float normalizedTime) => timeBar.fillAmount = normalizedTime;

        public void Present(IItemHolder itemHolder)
        {
            foreach (IItemPanel itemPanel in _itemPanels)
            {
                itemPanel.Present(itemHolder);
            }
        }

        public void Toggle(bool state) => notesPanel.SetActive(state);

        private void ToggleTimePanel(bool state) => timePanel.SetActive(state);

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
            {
                Destroy(customerMarker.gameObject);
            }

            _customerMarkers.Clear();
        }

        private void Init(CustomerOrder order, OrderData data)
        {
            _orders.Add(order.gameObject);
            order.Init(data);
        }

        private void GetReferences() => _itemPanels = GetComponents<IItemPanel>().Where(panel => panel.GetType() != typeof(ShopOpenedPanel)).ToArray();
    }
}