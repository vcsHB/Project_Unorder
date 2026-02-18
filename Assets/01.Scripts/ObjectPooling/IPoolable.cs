using UnityEngine;

namespace ObjectPooling
{
    public interface IPoolable
    {
        GameObject GameObject { get; }
        void OnPop();
        void OnPush();
    }
}
