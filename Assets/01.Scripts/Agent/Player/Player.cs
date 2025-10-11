using InputManage;
using Project_Unorder.AgentSytstem.PlayerManage.FSM;
using UnityEngine;
namespace Project_Unorder.AgentSytstem.PlayerManage
{

    public class Player : Agent
    {
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
        [SerializeField] private PlayerStateMachine _stateMachine;
        public PlayerStateMachine StateMachine => _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            // # Agent:Awake Component All Initialize -> stateMachine Initialize 
            _stateMachine = new PlayerStateMachine();
            _stateMachine.Initialize(this);

        }

        

    }

}