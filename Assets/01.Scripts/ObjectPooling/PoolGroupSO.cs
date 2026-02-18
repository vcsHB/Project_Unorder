using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    [CreateAssetMenu(fileName = "NewPoolGroup", menuName = "ObjectPool/Pool Group")]
    public class PoolGroupSO : ScriptableObject
    {
        public string groupName = "NewGroup";

        [HideInInspector] public string enumTypeName;

        public List<PoolItemData> items = new();
    }
}
