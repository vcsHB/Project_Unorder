using System;
using System.Collections.Generic;
using UnityEngine;
namespace Project_Unorder.AgentSytstem.PlayerManage.FSM
{

    public class PlayerStateMachine
    {

        private Dictionary<PlayerStateType, PlayerState> _stateDictionary;
        public PlayerState CurrentState { get; private set; }
        private Player _owner;

        public void Intialize(Player owner)
        {
            _owner = owner;

            foreach (PlayerStateType item in Enum.GetValues(typeof(PlayerStateType)))
            {

                AddState(item);
            }
        }

        public void AddState(PlayerStateType type)
        {
            Type t = Type.GetType($"Agents.Players.FSM.Player{type}State");
            PlayerState state = Activator.CreateInstance(t, _owner, this, 0) as PlayerState;
            _stateDictionary.Add(type, state);
        }

        public void ChangeState(PlayerStateType newStateType)
        {
            if (_stateDictionary.TryGetValue(newStateType, out PlayerState state))
            {
                CurrentState.Exit();
                CurrentState = state;
                CurrentState.Enter();
            }
        }
    }
}