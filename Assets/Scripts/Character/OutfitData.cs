using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class OutfitData
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Outfit Type { get; set; }
    }
}