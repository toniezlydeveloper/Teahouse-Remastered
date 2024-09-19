using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class SpeciesModel
    {
        [field: SerializeField] public GameObject Prefab { get; set; }
        [field: SerializeField] public Species Type { get; set; }
        [field: SerializeField] public List<Color> Colors { get; set; }
        [field: SerializeField] public Color DefaultColor { get; set; }
        [field: SerializeField] public List<string> Outfits { get; set; }
        [field: SerializeField] public string DefaultOutfit { get; set; }
    }
}