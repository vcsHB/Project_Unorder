using System;
using System.Collections.Generic;
using Project_Unorder.Core.Attribute;
using UnityEngine;

namespace Project_Unorder.AgentSytstem.PlayerManage.FSM
{
    [System.Serializable]
    public class PlayerStateMachine
    {

        private Dictionary<PlayerStateType, PlayerState> _stateDictionary;
        public PlayerState CurrentState { get; private set; }

        [SerializeField, ReadOnly] private string _currentStateDisplayString;
        private Player _owner;

        public void Initialize(Player owner)
        {
            _owner = owner;

            foreach (PlayerStateType item in Enum.GetValues(typeof(PlayerStateType)))
            {
                AddState(item);
            }
            if (_stateDictionary.TryGetValue(PlayerStateType.Idle, out PlayerState state))
            {
                CurrentState = state;
                _currentStateDisplayString = PlayerStateType.Idle.ToString();
                CurrentState.Enter();
            }
        }

        public void AddState(PlayerStateType type)
        {
            Type t = Type.GetType($"Project_Unorder.AgentSystem.PlayerManage.FSM.Player{type}State");
            PlayerState state = Activator.CreateInstance(t, _owner, this, 0) as PlayerState;
            _stateDictionary.Add(type, state);
        }

        public void ChangeState(PlayerStateType newStateType)
        {
            if (_stateDictionary.TryGetValue(newStateType, out PlayerState state))
            {
                CurrentState.Exit();
                CurrentState = state;
                _currentStateDisplayString = newStateType.ToString();
                CurrentState.Enter();
            }
        }
    }
}