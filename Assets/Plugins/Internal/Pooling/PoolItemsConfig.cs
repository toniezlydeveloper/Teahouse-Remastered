using UnityEngine;

namespace Internal.Pooling
{
    [CreateAssetMenu(menuName = "Config/Pooled Items")]
    public class PoolItemsConfig : ScriptableObject
    {
        [field: SerializeField] public PoolItem[] Prefabs { get; set; }
    }
}