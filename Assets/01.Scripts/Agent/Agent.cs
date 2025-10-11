using System;
using System.Collections.Generic;
using UnityEngine;
using Project_Unorder.CombatSystem;

namespace Project_Unorder.AgentSystem
{
    /// <summary>
    /// InGame ENTITY Default Class.  
    /// </summary>
    public class Agent : MonoBehaviour
    {
        private Dictionary<Type, IAgentComponent> _components = new Dictionary<Type, IAgentComponent>();

        public HealthBody HealthBody { get; private set; }
        public bool IsDead { get; protected set; }

        protected virtual void Awake()
        {
            HealthBody = GetComponent<HealthBody>();

            // # Agent Component Initialize
            AddComponentToDictionary();
            ComponentInitialize();
            ComponentAfterInitialize();

            // # Call Order 
            // Initialize() -> AfterInitialize() -> LateInitialize()
            // in Destroy : Dispose
        }

        protected virtual void Start()
        {
            ComponentLateInitialize();
        }

        public virtual void HandleAgentDie()
        {
            if (!IsDead)
            {
                IsDead = true;
            }
        }

        #region AgentComponent System

        private void AddComponentToDictionary()
        {
            var components = GetComponentsInChildren<IAgentComponent>(true);
            foreach (var component in components)
            {
                _components[component.GetType()] = component;
            }
        }

        private void ComponentInitialize()
        {
            foreach (var component in _components.Values)
            {
                component.Initialize(this);
            }
        }

        private void ComponentAfterInitialize()
        {
            foreach (var component in _components.Values)
            {
                component.AfterInitialize();
            }
        }

        private void ComponentLateInitialize()
        {
            foreach (var component in _components.Values)
            {
                component.LateInitialize();
            }
        }

        public T GetCompo<T>(bool allowDerived = false) where T : class
        {
            Type targetType = typeof(T);

            if (_components.TryGetValue(targetType, out IAgentComponent cached))
            {
                return cached as T;
            }

            T found = GetComponentInChildren<T>();
            if (found is IAgentComponent agentComponent)
            {
                _components[targetType] = agentComponent;
                return found;
            }

            if (allowDerived)
            {
                foreach (var kvp in _components)
                {
                    if (kvp.Key.IsSubclassOf(targetType) && kvp.Value is T derivedCompo)
                    {
                        return derivedCompo;
                    }
                }
            }

            return default;
        }

        #endregion
    }
}
