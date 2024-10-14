using System;
using System.Collections.Generic;
using System.Linq;
using Items.Implementations;
using UnityEngine;

namespace UI.Items
{
    public class AddInPanel : AItemPanel<IAddInsHolder>
    {
        [SerializeField] private Transform addInIconsParent;
        [SerializeField] private AddInIcon addInIconPrefab;

        private Dictionary<Enum, AddInIcon> _usedIcons = new();
        private List<AddInIcon> _unusedIcons = new();
        private List<Enum> _presentedAddIns = new();

        private static readonly List<Type> IngredientTypes = new()
        {
            typeof(WaterType),
            typeof(HerbType),
            typeof(FlowerType),
            typeof(TeabagType)
        };
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
            SortIcons();
        }
        
        private void GetMissingIcons(List<Enum> requiredAddIns)
        {
            foreach (Enum addIn in requiredAddIns)
            {
                if (IsAlreadyShowing(addIn))
                {
                    continue;
                }

                if (TryGetCachedIcon(out AddInIcon icon))
                {
                    Init(addIn, icon);
                }
                else
                {
                    Init(addIn, Instantiate(addInIconPrefab, addInIconsParent));
                }
            }
        }

        private bool TryGetRequiredAddIns(IAddInsHolder addIn, out List<Enum> requiredAddIns)
        {
            requiredAddIns = addIn != null ? addIn.HeldAddIns : EmptyAddIns;

            if (requiredAddIns.Count != _presentedAddIns.Count)
            {
                return true;
            }

            return !requiredAddIns.All(requiredAddIn => _presentedAddIns.Contains(requiredAddIn));
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

        private bool IsAlreadyShowing(Enum addIn) => _usedIcons.ContainsKey(addIn);

        private bool TryGetCachedIcon(out AddInIcon icon)
        {
            icon = null;
            
            if (_unusedIcons.Count < 1)
            {
                return false;
            }
            
            icon = _unusedIcons.First();
            icon.gameObject.SetActive(true);
            _unusedIcons.Remove(icon);
            return true;
        }

        private void Init(Enum addIn, AddInIcon icon)
        {
            _usedIcons.Add(addIn, icon);
            icon.Init(addIn);
        }

        private void Cache(List<Enum> requiredAddIns) => _presentedAddIns = new List<Enum>(requiredAddIns);

        private void SortIcons()
        {
            List<KeyValuePair<Enum, AddInIcon>> icons = _usedIcons.ToList();
            icons.Sort((icon1, icon2) => IngredientTypes.IndexOf(icon1.Key.GetType()) - IngredientTypes.IndexOf(icon2.Key.GetType()));
            
            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].Value.transform.SetSiblingIndex(i);
            }
        }
    }
}