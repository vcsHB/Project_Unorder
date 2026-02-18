using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public sealed class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [SerializeField] private List<PoolGroupSO> poolGroups = new();

        // enumType -> (enumValue -> Queue)
        private readonly Dictionary<Type, Dictionary<int, Queue<GameObject>>> _pools = new();

        // enumType -> PoolGroupSO
        private readonly Dictionary<Type, PoolGroupSO> _groupByType = new();

        // GameObject -> (enumTypeName, enumValue) for Push reverse-lookup
        private readonly Dictionary<GameObject, (string typeName, int value)> _registry = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var group in poolGroups)
            {
                if (string.IsNullOrEmpty(group.enumTypeName)) continue;

                var enumType = Type.GetType(group.enumTypeName);
                if (enumType == null)
                {
                    Debug.LogError($"[PoolManager] Enum type '{group.enumTypeName}' not found. Regenerate enum for group '{group.groupName}'.");
                    continue;
                }

                var poolDict = new Dictionary<int, Queue<GameObject>>();
                _pools[enumType] = poolDict;
                _groupByType[enumType] = group;

                var groupParent = new GameObject($"[Pool] {group.groupName}").transform;
                groupParent.SetParent(transform);

                foreach (var item in group.items)
                {
                    if (!ValidateItem(item, group.groupName)) continue;

                    if (!Enum.TryParse(enumType, item.itemName, out var enumObj))
                    {
                        Debug.LogError($"[PoolManager] Enum value '{item.itemName}' not found in {enumType.Name}. Regenerate enum.");
                        continue;
                    }

                    int enumValue = (int)enumObj;
                    var queue = new Queue<GameObject>();
                    poolDict[enumValue] = queue;

                    var itemParent = new GameObject(item.itemName).transform;
                    itemParent.SetParent(groupParent);

                    for (int i = 0; i < item.initialCount; i++)
                        queue.Enqueue(CreateInstance(item.prefab, group.enumTypeName, enumValue, itemParent));
                }
            }
        }

        private bool ValidateItem(PoolItemData item, string groupName)
        {
            if (item.prefab == null)
            {
                Debug.LogError($"[PoolManager] Group '{groupName}' item '{item.itemName}' has no prefab.");
                return false;
            }
            if (item.prefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError($"[PoolManager] Prefab '{item.prefab.name}' does not implement IPoolable.");
                return false;
            }
            return true;
        }

        private GameObject CreateInstance(GameObject prefab, string enumTypeName, int enumValue, Transform parent)
        {
            var go = Instantiate(prefab, parent);
            go.SetActive(false);

            var identity = go.GetComponent<PoolIdentity>() ?? go.AddComponent<PoolIdentity>();
            identity.EnumTypeName = enumTypeName;
            identity.EnumValue = enumValue;

            _registry[go] = (enumTypeName, enumValue);
            return go;
        }

        /// <summary>enum 키에 해당하는 풀 오브젝트를 꺼냅니다.</summary>
        public GameObject Pop<TEnum>(TEnum key) where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            int enumValue = Convert.ToInt32(key);

            if (!_pools.TryGetValue(enumType, out var poolDict) ||
                !poolDict.TryGetValue(enumValue, out var queue))
            {
                Debug.LogError($"[PoolManager] Pool not found for {enumType.Name}.{key}");
                return null;
            }

            GameObject go;
            if (queue.Count > 0)
            {
                go = queue.Dequeue();
            }
            else
            {
                var group = _groupByType[enumType];
                var item = group.items.Find(i => i.itemName == key.ToString());
                if (item?.prefab == null)
                {
                    Debug.LogError($"[PoolManager] Cannot expand pool for {enumType.Name}.{key}");
                    return null;
                }

                var parent = transform.Find($"[Pool] {group.groupName}/{key}");
                go = CreateInstance(item.prefab, group.enumTypeName, enumValue, parent ? parent : transform);
            }

            go.SetActive(true);
            go.GetComponent<IPoolable>().OnPop();
            return go;
        }

        /// <summary>IPoolable 오브젝트를 풀에 반환합니다.</summary>
        public void Push(IPoolable poolable)
        {
            var go = poolable.GameObject;
            if (!_registry.TryGetValue(go, out var info))
            {
                Debug.LogError("[PoolManager] Trying to push an object not managed by this pool.");
                return;
            }

            poolable.OnPush();
            go.SetActive(false);

            var enumType = Type.GetType(info.typeName);
            if (enumType != null && _pools.TryGetValue(enumType, out var poolDict) &&
                poolDict.TryGetValue(info.value, out var queue))
            {
                queue.Enqueue(go);
            }
        }
    }
}
