using System.Collections.Generic;
using Furniture;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using UI.Helpers;
using UnityEngine;

namespace UI.Core
{
    public interface IFurnishingPanel : IDependency
    {
        void Present(List<FurniturePiece> pieces);
        void Present(int selectedIndex);
    }

    public class BedroomPanel : AUIPanel, IFurnishingPanel
    {
        [SerializeField] private FurnitureSlot slotPrefab;
        [SerializeField] private Transform slotsParent;

        private List<FurnitureSlot> _slots = new();

        public void Present(List<FurniturePiece> pieces)
        {
            AddMissingSlots(pieces);
            SetupSlots(pieces);
        }

        public void Present(int selectedIndex)
        {
            for (int i = 0; i < _slots.Count; i++)
                _slots[i].Present(i == selectedIndex);
        }

        private void AddMissingSlots(List<FurniturePiece> pieces)
        {
            int missingSlotCount = pieces.Count - _slots.Count;
            
            if (missingSlotCount == 0)
                return;
            
            for (int i = 0; i < missingSlotCount; i++)
                _slots.Add(Instantiate(slotPrefab, slotsParent));
        }

        private void SetupSlots(List<FurniturePiece> pieces)
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