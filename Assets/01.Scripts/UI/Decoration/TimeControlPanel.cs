using Project_Unorder.Core.UtilSystem;
using UnityEngine;
namespace Project_Unorder.UIManage.Decoration
{

    public class TimeControlPanel : MonoBehaviour
    {
        [SerializeField] private float _newTimeScale = 1f;
        public void ApplyControl()
        {
            TimeManager.AddTimeScaleRecord(_newTimeScale);
        }

        public void RemoveControl()
        {
            TimeManager.RemoveTimeScaleRecord();
        }
    }
}