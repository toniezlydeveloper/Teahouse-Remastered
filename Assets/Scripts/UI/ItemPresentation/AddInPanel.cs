using System;
using System.Collections.Generic;
using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace UI.ItemPresentation
{
    public class AddInPanel : AItemPanel<IAddInsHolder>
    {
        [SerializeField] private Transform addInIconsParent;
        [SerializeField] private AddInIcon addInIconPrefab;

        private Dictionary<Enum, AddInIcon> _usedIcons = new();
        private List<AddInIcon> _unusedIcons = new();
        private List<Enum> _presentedAddIns;
        private int _presentedAddInCount;

        private static readonly List<Enum> EmptyAddIns = new();
        
        private void Update()
        {
            TryGetItem(out IAddInsHolder addIn);

            if (!TryGetRequiredAddIns(addIn, out List<Enum> requiredAddIns))
            {
                return;
            }
            
            DisableUnusedIcons(requiredAddIns);
            GetMissingIcons(requiredAddIns);
            Cache(requiredAddIns);
        }

        private bool TryGetRequiredAddIns(IAddInsHolder addIn, out List<Enum> requiredAddIns)
        {
            requiredAddIns = addIn != null ? addIn.HeldAddIns : EmptyAddIns;
            return _presentedAddIns != requiredAddIns || _presentedAddInCount != requiredAddIns.Count;
        }

        private void DisableUnusedIcons(List<Enum> requiredAddIns)
        {
            List<KeyValuePair<Enum, AddInIcon>> addInsToRemove = _usedIcons.Where(addIn => !requiredAddIns.Contains(addIn.Key)).ToList();
            List<AddInIcon> iconsToHide = addInsToRemove.Select(addIn => addIn.Value).ToList();
            
            foreach (AddInIcon icon in iconsToHide)
            {
                icon.gameObject.SetActive(false);
            }
            
            foreach (KeyValuePair<Enum,AddInIcon> addIn in addInsToRemove)
            {
                _usedIcons.Remove(addIn.Key);
            }
            
            _unusedIcons.AddRange(iconsToHide);
        }
        
        private void GetMissingIcons(List<Enum> requiredAddIns)
        {
            foreach (Enum addIn in requiredAddIns)
            {
                if (_usedIcons.ContainsKey(addIn))
                {
                    continue;
                }

                AddInIcon icon;

                if (_unusedIcons.Count > 0)
                {
                    icon = _unusedIcons.First();
                    _unusedIcons.Remove(icon);
                    icon.gameObject.SetActive(true);
                }
                else
                {
                    icon = Instantiate(addInIconPrefab, addInIconsParent);
                }
                
                _usedIcons.Add(addIn, icon);
                icon.Init(addIn);
            }

        }

        private void Cache(List<Enum> requiredAddIns)
        {
            _presentedAddInCount = requiredAddIns.Count;
            _presentedAddIns = requiredAddIns;
        }
    }
}