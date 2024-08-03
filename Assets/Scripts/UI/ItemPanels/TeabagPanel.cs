using System;
using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ItemPanels
{
    public class TeabagPanel : AItemPanel<ITeabagItem>
    {
        [SerializeField] private Image teabagTypeIndicator;

        private void Update()
        {
            TryGetItem(out ITeabagItem teabagItem);
            RefreshColor(teabagItem);
        }

        private void RefreshColor(ITeabagItem teabagItem)
        {
            switch (teabagItem?.TeabagType)
            {
                case TeabagType.None:
                    ToggleIndicator(false);
                    break;
                case TeabagType.Default:
                    ToggleIndicator(true);
                    ToggleColor(Color.magenta);
                    break;
                case TeabagType.Lavender:
                    ToggleIndicator(true);
                    ToggleColor(Color.red);
                    break;
                case null:
                    ToggleIndicator(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ToggleIndicator(bool state)
        {
            if (teabagTypeIndicator.gameObject.activeSelf == state)
                return;
            
            teabagTypeIndicator.gameObject.SetActive(state);
        }

        private void ToggleColor(Color color)
        {
            if (teabagTypeIndicator.color == color)
                return;

            teabagTypeIndicator.color = color;
        }
    }
}