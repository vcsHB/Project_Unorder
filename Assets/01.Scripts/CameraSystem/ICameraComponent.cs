using UnityEngine;
namespace Project_Unorder.CameraSystem
{

    public interface ICameraComponent
    {
        public void Initialize(CameraManager manager);
        public void OnEnable();
        public void OnDisable();
    }
}