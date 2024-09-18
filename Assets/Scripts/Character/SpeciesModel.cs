using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class SpeciesModel
    {
        [field: SerializeField] public GameObject Prefab { get; set; }
        [field: SerializeField] public Species Type { get; set; }
    }
}