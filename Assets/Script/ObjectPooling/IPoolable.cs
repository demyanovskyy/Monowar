using System;
using UnityEngine;

namespace Assets.Scripts.Core.ObjectPooling
{
    public interface IPoolable
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
        event Action<IPoolable> OnReturnToPool;


        void Reset();
    }
}