using System;
using UnityEngine;

namespace ObjectPooling
{
    [Serializable]
    public class PoolItemData
    {
        public string itemName = "NewItem";
        [TextArea(1, 3)] public string description;
        public int initialCount = 10;
        public GameObject prefab;
    }
}
