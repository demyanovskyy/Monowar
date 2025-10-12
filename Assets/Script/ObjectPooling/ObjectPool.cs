using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.ObjectPooling
{
    public class ObjectPool
    {
        //private static ObjectPool instance;
        // public static ObjectPool Instance => instance ??= new ObjectPool();

        private readonly Dictionary<Component, PoolTask> _activePoolTasks;
        private Transform _objectPoolTransform;



        public ObjectPool()
        {
            _activePoolTasks = new Dictionary<Component, PoolTask>();
            _objectPoolTransform = new GameObject().transform;
            _objectPoolTransform.name = nameof(ObjectPool);
        }

        private void AddTaskToPool<T>(T prefab, out PoolTask poolTask) where T : Component, IPoolable
        {
            GameObject container = new GameObject
            {
                name = $"{prefab.name}s_pool"
            };
            container.transform.SetParent((_objectPoolTransform));
            poolTask = new PoolTask(container.transform);
            _activePoolTasks.Add(prefab, poolTask);
        }

        public T GetObject<T>(T prefab) where T : Component, IPoolable
        {

            if (!_activePoolTasks.TryGetValue(prefab, out var poolTask))
            {
                AddTaskToPool(prefab, out poolTask);
            }

            return poolTask.GetFreeObject(prefab);
        }

        public void DisposeTask()
        {
            foreach (var poolTask in _activePoolTasks.Values)
            {
                poolTask.ClearPool();
            }
        }
    }
}