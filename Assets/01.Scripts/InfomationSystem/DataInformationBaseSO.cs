using UnityEngine;
using UnityEngine.Localization;
namespace Project_Unorder.Information
{

    public abstract class DataInformationBaseSO : ScriptableObject
    {
        public uint tid;
        public DataCategoryType category;
        public LocalizedString dataName;
        public LocalizedString description;


#if UNITY_EDITOR
        internal void SetTid(uint newTid)
        {
            tid = newTid;
        }


#endif

    }
}