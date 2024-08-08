using System.Collections.Generic;
using Internal.Dependencies.Core;
using UI.Helpers;
using UnityEngine;

namespace UI.Shared
{
    public interface IFurnishingPanel : IDependency
    {
        void Present(List<FurniturePieceData> pieces);
        void Present(int selectedIndex);
        void Present(bool state);
    }

    public class FurniturePieceData
    {
        public Sprite Icon { get; set; }
        public int Count { get; set; }
    }
    
    public class FurnishingPanel : MonoBehaviour, IFurnishingPanel
    {
        [SerializeField] private bool shouldPresentSelection;
        [SerializeField] private FurnitureSlot slotPrefab;
        [SerializeField] private Transform slotsParent;

        private List<FurnitureSlot> _slots = new();

        public void Present(List<FurniturePieceData> pieces)
        {
            AddMissingSlots(pieces);
            SetupSlots(pieces);
        }

        public void Present(int selectedIndex)
        {
            if (!shouldPresentSelection)
                return;
            
            for (int i = 0; i < _slots.Count; i++)
                _slots[i].Present(i == selectedIndex);
        }

        public void Present(bool state) => gameObject.SetActive(state);

        private void AddMissingSlots(List<FurniturePieceData> pieces)
        {
            int missingSlotCount = pieces.Count - _slots.Count;
            
            if (missingSlotCount == 0)
                return;
            
            for (int i = 0; i < missingSlotCount; i++)
                _slots.Add(Instantiate(slotPrefab, slotsParent));
        }

        private void SetupSlots(List<FurniturePieceData> pieces)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] != null)
                    _slots[i].Present(pieces[i].Icon, pieces[i].Count);
                else
                    _slots[i].Present(null, null);
            }
        }
    }
}