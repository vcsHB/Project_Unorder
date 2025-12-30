using UnityEngine;

namespace Project_Unorder.CameraSystem
{
    public abstract class CameraComponentBase : MonoBehaviour, ICameraComponent
    {
        protected CameraManager Manager { get; private set; }

        public virtual void Initialize(CameraManager manager)
        {
            Manager = manager;
        }

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}