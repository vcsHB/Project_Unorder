using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Project_Unorder.VolumeManage
{
    /// <summary>
    ///
    /// </summary>
    public class VolumeManager : MonoSingleton<VolumeManager>, IEarlyAwakeableManager
    {
        [SerializeField] private Volume _globalVolume;

        private readonly Dictionary<Type, IVolumeComponent> _components = new();

        public Volume GlobalVolume => _globalVolume;

        public void PreAwake()
        {
            Debug.Assert(_globalVolume != null, "[VolumeManager] Global Volume is not Bind");

            RegisterComponents();
            InitializeComponents();
        }

        private void RegisterComponents()
        {
            var comps = GetComponentsInChildren<IVolumeComponent>(true);
            foreach (var comp in comps)
            {
                var type = comp.GetType();
                if (!_components.ContainsKey(type))
                {
                    _components[type] = comp;
                }
            }
        }

        private void InitializeComponents()
        {
            foreach (var comp in _components.Values)
            {
                comp.Initialize(this);
            }
        }

        public T GetCompo<T>() where T : class, IVolumeComponent
        {
            if (_components.TryGetValue(typeof(T), out var comp))
            {
                return comp as T;
            }
            return null;
        }
    }
}