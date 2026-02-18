using UnityEngine;

namespace ObjectPooling
{
    [DisallowMultipleComponent]
    internal sealed class PoolIdentity : MonoBehaviour
    {
        internal string EnumTypeName;
        internal int EnumValue;
    }
}
