using System;
using UnityEngine;

namespace Customers
{
    [Serializable]
    public class SpawnData
    {
        [field:SerializeField] public float SpawnInterval { get; set; }
        [field:SerializeField] public int SpawnCount { get; set; }

        public float Duration => SpawnInterval * SpawnCount;
    }
}