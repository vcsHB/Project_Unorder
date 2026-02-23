using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Project_Unorder.Information
{
    [CustomEditor(typeof(DataGroupSO))]
    public class CustomDataGroupSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            if (GUILayout.Button("Auto Collect & Sort by Category", GUILayout.Height(30)))
            {
                CollectAllDatas();
            }
        }

        private void CollectAllDatas()
        {
            DataGroupSO groupSO = (DataGroupSO)target;

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(DataInformationBaseSO)}");

            var collected = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<DataInformationBaseSO>(
                    AssetDatabase.GUIDToAssetPath(guid)))
                .Where(so => so != null)
                .OrderBy(so => so.category)   // category enum Sorting
                .ToArray();

            Undo.RecordObject(groupSO, "Auto Collect DataInformationBaseSO");
            groupSO.datas = collected;

            EditorUtility.SetDirty(groupSO);
            AssetDatabase.SaveAssets();

            Debug.Log($"[DataGroupSO] Information Collection Complete. amount:{collected.Length} (with category sorting)");
        }
    }
}