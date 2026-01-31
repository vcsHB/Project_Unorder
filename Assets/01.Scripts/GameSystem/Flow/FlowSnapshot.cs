using UnityEngine;

namespace Project_Unorder.FlowSystem
{

    public class FlowSnapshot : FlowData
    {
        public FlowSnapshot(uint flowChapter, uint step, uint mapId, uint layerIndex, Vector2 position)
        {
            this.flowChapter = flowChapter;
            this.step = step;
            this.mapId = mapId;
            this.layerIndex = layerIndex;
            this.position = position;

        }
    }
}