using UnityEngine;

namespace Project_Unorder.CameraSystem
{
    public class CameraFollowComponent : CameraComponentBase
    {
        [SerializeField] private Transform _target;
        private Transform _defaultFollowTarget = null;
        public override void Initialize(CameraManager manager)
        {
            base.Initialize(manager);
            _target = manager.MainCamera.Follow;
            _defaultFollowTarget = _target;
            if(_target == null)
            {
                Debug.LogWarning("Default Camera:FollowTarget is Not Binded");
            }
        }

        public void SetFollowDefault()
        {
            SetFollow(_defaultFollowTarget);
        }

        public void SetFollow(Transform target)
        {
            _target = target;
            Manager.MainCamera.Follow = target;
        }
    }
}
