using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class OutfitModel
    {
        [field: SerializeField] public GameObject Prefab { get; set; }
        [field: SerializeField] public Outfit Type { get; set; }
    }
}