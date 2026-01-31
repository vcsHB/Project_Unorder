using UnityEngine;
namespace Project_Unorder.FlowSystem
{
    [System.Serializable]
    public class FlowData
    {
        public bool isChapterStarted;
        public uint flowChapter;
        public uint step;
        public uint mapId;
        public uint layerIndex;
        public Vector2 position;

        
        
    }
}