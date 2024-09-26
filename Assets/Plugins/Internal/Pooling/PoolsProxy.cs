using System;
using System.Collections.Generic;
using Internal.Dependencies.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Internal.Pooling
{
    public interface IPoolsProxy : IDependency
    {
        TObject GetTyped<TObject>(string key, Vector3 position, Quaternion rotation, Transform parent = null);
        GameObject Get(string key, Vector3 position, Quaternion rotation, Transform parent = null);
        void Release(string key, GameObject item);
    }

    public class PoolsProxy : MonoBehaviour, IPoolsProxy
    {
        [SerializeField] private PoolItemsConfig[] configs;

        private readonly Dictionary<string, ObjectPool<PoolItem>> _poolsByKey = new();

        private void Awake() => InitPools();

        public TObject GetTyped<TObject>(string key, Vector3 position, Quaternion rotation, Transform parent = null) =>
            Get(key, position, rotation, parent).GetComponent<TObject>();

        public GameObject Get(string key, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            PoolItem item = _poolsByKey[key].Get();
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.transform.parent = parent;
            return item.gameObject;
        }

        public void Release(string key, GameObject item) => _poolsByKey[key].Release(item.GetComponent<PoolItem>());

        private void InitPools()
        {
            foreach (PoolItemsConfig config in configs)
            {
                foreach (PoolItem prefab in config.Prefabs)
                {
                    Action<PoolItem> releaseCallback = item =>
                    {
                        if (item.TryGetComponent(out PoolItem poolItem))
                            poolItem.ShouldRelease = false;
                    
                        item.transform.SetParent(transform);
                        item.gameObject.SetActive(false);
                    };
                    Action<PoolItem> getCallback = item =>
                    {
                        item.ShouldRelease = true;
                        item.gameObject.SetActive(true);
                    };
                    Func<PoolItem> createCallback = () =>
                    {
                        PoolItem item = Instantiate(prefab);
                        item.ShouldRelease = true;
                        return item;
                    };

                    _poolsByKey.Add(prefab.name, new ObjectPool<PoolItem>(createCallback, getCallback, releaseCallback));
                }
            }
        }
    }
}