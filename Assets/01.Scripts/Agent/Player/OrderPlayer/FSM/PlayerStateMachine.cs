using System;
using System.Collections.Generic;
using Project_Unorder.Core.Attribute;
using UnityEngine;

namespace Project_Unorder.AgentSystem.PlayerManage.FSM
{
    [System.Serializable]
    public class PlayerStateMachine
    {

        private Dictionary<PlayerStateType, OrderPlayerState> _stateDictionary;
        public OrderPlayerState CurrentState { get; private set; }

        [SerializeField, ReadOnly] private string _currentStateDisplayString;
        private OrderPlayer _owner;

        public void Initialize(OrderPlayer owner)
        {
            _owner = owner;
            _stateDictionary = new();
            foreach (PlayerStateType item in Enum.GetValues(typeof(PlayerStateType)))
            {
                AddState(item);
            }
            if (_stateDictionary.TryGetValue(PlayerStateType.Idle, out OrderPlayerState state))
            {
                CurrentState = state;
                _currentStateDisplayString = PlayerStateType.Idle.ToString();
                CurrentState.Enter();
            }
        }

        public void AddState(PlayerStateType type)
        {
            Type t = Type.GetType($"Project_Unorder.AgentSystem.PlayerManage.FSM.Player{type}State");
            OrderPlayerState state = Activator.CreateInstance(t, _owner, this, 0) as OrderPlayerState;
            _stateDictionary.Add(type, state);
        }

        public void ChangeState(PlayerStateType newStateType)
        {
            if (_stateDictionary.TryGetValue(newStateType, out OrderPlayerState state))
            {
                CurrentState.Exit();
                CurrentState = state;
                _currentStateDisplayString = newStateType.ToString();
                CurrentState.Enter();
            }
        }

        public void UpdateState()
        {
            CurrentState.UpdateState();
        }
    }
}