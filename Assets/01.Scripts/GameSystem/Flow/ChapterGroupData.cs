using UnityEngine;
namespace Project_Unorder.FlowSystem
{
    [CreateAssetMenu(menuName = "SO/Flow/ChapterGroup")]
    public class ChapterGroupData : ScriptableObject
    {
        public ChapterData[] chapters;
        
    }
}