using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(menuName = "Config/Add Ins")]
    public class AddInsConfig : ScriptableObject
    {
        [field:SerializeField] public List<AddInIcon> Icons { get; set; }
        [field:SerializeField] public List<TeabagColor> TeabagColors { get; set; }
        [field:SerializeField] public List<HerbColor> HerbColors { get; set; }
        [field:SerializeField] public List<FlowerColor> FlowerColors { get; set; }
    }

    [Serializable]
    public class AddInIcon
    {
        [field:SerializeField] public string TypeName { get; set; }
        [field:SerializeField] public Sprite Icon { get; set; }
    }

    [Serializable]
    public class TeabagColor : AddInColor<TeabagType>
    {
    }

    [Serializable]
    public class HerbColor : AddInColor<HerbType>
    {
    }

    [Serializable]
    public class FlowerColor : AddInColor<FlowerType>
    {
    }

    [Serializable]
    public class AddInColor<TAddIn>
    {
        [field:SerializeField] public TAddIn AddInType { get; set; }
        [field:SerializeField] public Color Color { get; set; }
    }
}