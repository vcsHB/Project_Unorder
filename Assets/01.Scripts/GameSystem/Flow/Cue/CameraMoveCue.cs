using Project_Unorder.CameraSystem;
using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public class CameraMoveCue : MoveObjectCue
    {
        private void Awake()
        {
            _targetTrm = CameraManager.Instance.GetCompo<CameraFollowComponent>().Target;
            if(_targetTrm  == null)
            {
                Debug.Log("CameraMoveCue : Default Follow Target is not Binded");
                return;
            }            
        }

    }
}