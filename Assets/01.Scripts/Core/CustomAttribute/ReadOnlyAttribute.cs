using System;
using UnityEngine;

namespace Project_Unorder.Core.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public ReadOnlyAttribute() { }
    }
}