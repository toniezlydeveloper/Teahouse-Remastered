using System;
using Items.Implementations;
using UnityEngine;

namespace Items.Models
{
    public class CupModel : AItemModel
    {
        [SerializeField] private GameObject waterIndicator;
        [SerializeField] private GameObject teabagIndicator;
        [SerializeField] private GameObject dirtyIndicator;
        
        public override void Refresh(object item)
        {
            if (item is not Cup cup)
                return;

            waterIndicator.SetActive(cup.HasWater);
            waterIndicator.GetComponent<Renderer>().material.color =
                Color.LerpUnclamped(Color.cyan, Color.blue, cup.WaterTemperature / 100f);

            switch (cup.TeabagType)
            {
                case TeabagType.None:
                    teabagIndicator.SetActive(false);
                    break;
                case TeabagType.Default:
                    teabagIndicator.SetActive(true);
                    teabagIndicator.GetComponent<Renderer>().material.color = Color.magenta;
                    break;
                case TeabagType.Lavender:
                    teabagIndicator.SetActive(true);
                    teabagIndicator.GetComponent<Renderer>().material.color = Color.red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            dirtyIndicator.SetActive(cup.IsDirty);
        }
    }
}