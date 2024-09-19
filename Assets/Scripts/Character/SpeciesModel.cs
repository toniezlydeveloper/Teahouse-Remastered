using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class SpeciesModel
    {
        [field: SerializeField] public CharacterModel Prefab { get; set; }
        [field: SerializeField] public Species Type { get; set; }
        [field: SerializeField] public List<Color> Colors { get; set; }
        [field: SerializeField] public int DefaultColorIndex { get; set; }
        [field: SerializeField] public List<string> Outfits { get; set; }
        [field: SerializeField] public int DefaultOutfitIndex { get; set; }
    }
}