using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Project_Unorder.CameraSystem
{
    public class CameraManager : MonoSingleton<CameraManager>, IEarlyAwakeableManager
    {
        [SerializeField] private CinemachineCamera _mainCamera;

        private readonly Dictionary<Type, ICameraComponent> _components = new();

        public CinemachineCamera MainCamera => _mainCamera;


        public void PreAwake()
        {
            
            Debug.Assert(_mainCamera);

            RegisterComponents();
            InitializeComponents();
        }

        private void RegisterComponents()
        {
            var comps = GetComponentsInChildren<ICameraComponent>(true);
            foreach (var comp in comps)
            {
                _components[comp.GetType()] = comp;
            }
        }

        private void InitializeComponents()
        {
            foreach (var comp in _components.Values)
            {
                comp.Initialize(this);
            }
        }

        public T GetCompo<T>() where T : class, ICameraComponent
        {
            if (_components.TryGetValue(typeof(T), out ICameraComponent comp))
            {
                return comp as T;

            }
            return null;
        }


    }
}
