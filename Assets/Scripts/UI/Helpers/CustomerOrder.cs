using System;
using System.Linq;
using UI.Core;
using UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class CustomerOrder : MonoBehaviour
    {
        [SerializeField] private Image speciesIconContainer;
        [SerializeField] private Transform addInIconsParent;
        [SerializeField] private AddInIcon addInIconPrefab;
        [SerializeField] private IconSet speciesIcons;
        [SerializeField] private GameObject emptyText;

        public void Init(OrderData data)
        {
            foreach (Enum addIn in data.AddIns)
            {
                Instantiate(addInIconPrefab, addInIconsParent).Init(addIn);
            }
            
            InitEmptyText(data);
            InitIcon(data);
        }

        private void InitEmptyText(OrderData data) => emptyText.SetActive(data.AddIns.Count == 0);
        
        private void InitIcon(OrderData data) => speciesIconContainer.sprite = speciesIcons.Value.First(icon => icon.name.Contains(data.Species.ToString()));
    }
}