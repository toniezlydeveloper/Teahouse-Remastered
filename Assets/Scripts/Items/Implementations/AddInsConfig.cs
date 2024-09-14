using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(menuName = "Setup/Add Ins")]
    public class AddInsConfig : ScriptableObject
    {
        [field:SerializeField] public List<HerbColor> HerbColors { get; set; }
        [field:SerializeField] public List<FlowerColor> FlowerColors { get; set; }
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
        [field:SerializeField] public TAddIn AddIn { get; set; }
        [field:SerializeField] public Color Color { get; set; }
    }
}