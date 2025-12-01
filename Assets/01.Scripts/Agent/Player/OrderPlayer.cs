using InputManage;
using Project_Unorder.AgentSystem.PlayerManage.FSM;
using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{

    public class OrderPlayer : Agent
    {
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
        [SerializeField] private PlayerStateMachine _stateMachine;
        public PlayerStateMachine StateMachine => _stateMachine;

        protected override void Awake()
        {
            base.Awake();

        }

        protected override void Start()
        {
            base.Start();
            // # Agent:Awake Component All Initialize -> stateMachine Initialize 
            _stateMachine = new PlayerStateMachine();
            _stateMachine.Initialize(this);

        }

        private void Update()
        {
            _stateMachine.UpdateState();
        }



    }

}