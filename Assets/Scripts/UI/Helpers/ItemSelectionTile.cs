using System;
using System.Linq;
using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class ItemSelectionTile : MonoBehaviour
    {
        [SerializeField] private Color disabledColor;
        [SerializeField] private Color enabledColor;
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Image selection;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private Image back;

        public void Init(Enum value)
        {
            OverrideIcon(GetSprite(value));
            ColorBackground(GetColor(value));
            ToggleIcons(value);
        }

        public void Init(bool state) => selection.color = state ? enabledColor : disabledColor;

        private Sprite GetSprite(Enum value) =>  config.Icons.First(addInIcon => addInIcon.TypeName == value.GetType().Name).Icon;

        private Color? GetColor(Enum value) => value switch
        {
            HerbType => config.HerbColors.FirstOrDefault(herbColor => herbColor.AddInType == (HerbType)value)?.Color,
            FlowerType => config.FlowerColors.FirstOrDefault(flowerColor => flowerColor.AddInType == (FlowerType)value)?.Color,
            TeabagType => config.TeabagColors.FirstOrDefault(teabagColor => teabagColor.AddInType == (TeabagType)value)?.Color,
            WaterType => config.WaterColors.FirstOrDefault(waterColor => waterColor.AddInType == (WaterType)value)?.Color,
            _ => Color.magenta
        };

        private void OverrideIcon(Sprite sprite) => icon.sprite = sprite;

        private void ColorBackground(Color? color) => background.color = color ?? Color.black;

        private void ToggleIcons(Enum value)
        {
            bool isDefault = (int)(object)value == 0;
            icon.gameObject.SetActive(!isDefault);
            back.gameObject.SetActive(isDefault);
        }
    }
}