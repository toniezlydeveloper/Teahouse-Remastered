using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Utilities
{
    public class GameObjectProxy : MonoBehaviour
    {
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        private static readonly Dictionary<Type, GameObjectProxy> ProxiesByOwnerType = new();
        
        private void Start() => DontDestroyOnLoad(gameObject);

        public static GameObjectProxy Get<TOwner>()
        {
            if (ProxiesByOwnerType.TryGetValue(typeof(TOwner), out GameObjectProxy proxy))
            {
                return proxy;
            }
            
            proxy = new GameObject($"[Proxy] {typeof(TOwner).Name}").AddComponent<GameObjectProxy>();
            ProxiesByOwnerType.Add(typeof(TOwner), proxy);
            return proxy;
        }
    }
}