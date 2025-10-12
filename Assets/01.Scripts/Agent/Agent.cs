using System;
using System.Collections.Generic;
using UnityEngine;
using Project_Unorder.CombatSystem;
using System.Linq;

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

        public T GetCompo<T>(bool isDerived = false) where T : class
        {
            if (_components.TryGetValue(typeof(T), out IAgentComponent compo))
            {
                return compo as T;
            }
            else
            {
                //Debug.Log("Not Exist In components dictionary");
                T newComponent = GetComponentInChildren<T>();
                if (newComponent is IAgentComponent)
                {

                    _components.Add(typeof(T), newComponent as IAgentComponent);
                    //Debug.Log("Insert In dictionary");
                    return newComponent;
                }
            }

            if (!isDerived) return default;

            Type findType = _components.Keys.FirstOrDefault(x => x.IsSubclassOf(typeof(T)));
            if (findType != null)
                return _components[findType] as T;

            return default(T);
        }
        #endregion
    }
}
