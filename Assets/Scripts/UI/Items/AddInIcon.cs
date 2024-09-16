using System;
using System.Linq;
using Items.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Items
{
    public class AddInIcon : MonoBehaviour
    {
        [SerializeField] private AddInsConfig config;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;

        public void Init(Enum value)
        {
            OverrideIcon(GetSprite(value));
            ColorBackground(GetColor(value));
        }

        private Sprite GetSprite(Enum value) =>  config.Icons.First(addInIcon => addInIcon.TypeName == value.GetType().Name).Icon;

        private Color GetColor(Enum value) => value switch
        {
            HerbType => config.HerbColors.First(herbColor => herbColor.AddInType == (HerbType)value).Color,
            FlowerType => config.FlowerColors.First(flowerColor => flowerColor.AddInType == (FlowerType)value).Color,
            TeabagType => config.TeabagColors.First(teabagColor => teabagColor.AddInType == (TeabagType)value).Color,
            WaterType => config.WaterColors.First(waterColor => waterColor.AddInType == (WaterType)value).Color,
            _ => Color.magenta
        };

        private void OverrideIcon(Sprite sprite) => icon.sprite = sprite;

        private void ColorBackground(Color color) => background.color = color;
    }
}