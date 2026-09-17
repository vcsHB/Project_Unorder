using System;
using Project_Unorder.CombatSystem;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem
{
    [RequireComponent(typeof(HealthBody), typeof(BossPatternRunner))]
    public class Boss : Agent
    {
        public event Action<int> OnPhaseChangedEvent;

        public BossEncounterController EncounterController { get; private set; }
        public BossPatternRunner PatternRunner { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            PatternRunner = GetCompo<BossPatternRunner>();
        }

        public void BeginBattle(BossEncounterController controller)
        {
            EncounterController = controller;
            PatternRunner.StartRunning(controller);
        }

        public void EndBattle()
        {
            PatternRunner.StopRunning();
        }

        public void OnPhaseChanged()
        {
            PatternRunner.NotifyPhaseChanged();
            if (EncounterController != null)
                OnPhaseChangedEvent?.Invoke(EncounterController.CurrentPhaseIndex);
        }
    }
}
