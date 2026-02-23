using UnityEngine;

namespace Project_Unorder.Information
{
    [CreateAssetMenu(menuName = "SO/Information/DataGroupSO")]
    public class DataGroupSO : ScriptableObject
    {
        public DataInformationBaseSO[] datas;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (datas == null)
            {
                Debug.LogError("Data Array is Null");
                return;
            }
            for (uint i = 0; i < datas.Length; i++)
            {
                if (datas[i] == null)
                {
                    Debug.LogError($"Index:{i} Data is null");
                    return;
                }
                datas[i].SetTid(i);

            }

        }
#endif
    }
}