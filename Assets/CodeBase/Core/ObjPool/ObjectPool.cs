using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CodeBase.Core.ObjPool
{
    public class ObjectPool
    {
        private Dictionary<Type, List<IPoolableObject>> _pools = new Dictionary<Type, List<IPoolableObject>>();
        private Dictionary<Type, GameObject> _prefabs = new Dictionary<Type, GameObject>();
    
        private DiContainer _container;

        public ObjectPool(DiContainer container)
        {
            _container = container;
        }
    
        public void RegisterPrefab<T>(GameObject prefab, int preLoadCount) where T : IPoolableObject
        {
            Type objectType = typeof(T);

            if (!_prefabs.ContainsKey(objectType))
            {
                _prefabs.Add(objectType, prefab);
            }

            if (!_pools.TryGetValue(objectType, out var pool))
            {
                pool = new List<IPoolableObject>();
                _pools[objectType] = pool;
            }

            for (int i = 0; i < preLoadCount; i++)
            {
                if (pool.Count < preLoadCount)
                {
                    var newObj = CreateNewObject<T>(prefab);
                    pool.Add(newObj);
                }
            }
        }
        
        public T GetObject<T>() where T : IPoolableObject
        {
            Type objectType = typeof(T);

            if (!_pools.TryGetValue(objectType, out var pool))
            {
                pool = new List<IPoolableObject>();
                _pools[objectType] = pool;
            }

            foreach (var obj in pool.Where(obj => !obj.IsActive))
            {
                obj.Activate();
                return (T)obj;
            }

            if (_prefabs.TryGetValue(objectType, out var prefab))
            {
                var newObj = CreateNewObject<T>(prefab);
                pool.Add(newObj);
                
                return newObj;
            }

            Debug.LogError($"No prefab registered for type {objectType}");
            return default;
        }
        
        public void ReturnObject(IPoolableObject obj)
        {
            obj.Deactivate();
        }
        
        private T CreateNewObject<T>(GameObject prefab) where T : IPoolableObject
        {
            var newObjGameObject = _container.InstantiatePrefab(prefab);
            var newObj = newObjGameObject.GetComponent<T>();

            if (newObj == null)
            {
                Debug.LogError($"Prefab for type {typeof(T)} does not implement IPoolableObject");
            }

            newObj.Deactivate();
            return newObj;
        }
    }
}