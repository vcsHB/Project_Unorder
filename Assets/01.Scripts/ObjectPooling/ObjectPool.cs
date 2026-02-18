using System;
using UnityEngine;

namespace ObjectPooling
{
    public static class ObjectPool
    {
        /// <summary>
        /// 풀에서 오브젝트를 꺼냅니다.
        /// <para>예시: ObjectPool.Pop(BulletPoolKey.SmallBullet)</para>
        /// </summary>
        public static GameObject Pop<TEnum>(TEnum key) where TEnum : Enum
            => PoolManager.Instance.Pop(key);

        /// <summary>
        /// 오브젝트를 풀에 반환합니다.
        /// <para>예시: ObjectPool.Push(this) — MonoBehaviour가 IPoolable을 구현한 경우</para>
        /// </summary>
        public static void Push(IPoolable poolable)
            => PoolManager.Instance.Push(poolable);
    }
}
