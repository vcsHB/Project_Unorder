using Project_Unorder.CombatSystem.Core;
using Project_Unorder.FlowSystem;
using UnityEngine;

namespace Project_Unorder.BootSystem
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private BootProfile _profile;
        [Header("Targets")]
        [SerializeField] private FlowManager _flowManager;
        [SerializeField] private CombatManager _combatManager;

        public BootProfile Profile => _profile;

        private void Awake()
        {
            if (_profile == null || !_profile.OverridesSceneFlow) return;

            CancelSceneFlowAutoStart();
        }

        private void Start()
        {
            Boot();
        }

        private void CancelSceneFlowAutoStart()
        {
            FlowStep[] steps = FindObjectsByType<FlowStep>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < steps.Length; i++)
                steps[i].CancelAutoStart();
        }

        private void Boot()
        {
            if (_profile == null)
            {
                Debug.LogWarning("GameBootstrapper: BootProfile is not binded. Scene keeps its own flow");
                return;
            }

            switch (_profile.mode)
            {
                case BootMode.CombatOnly:
                    BootCombatOnly();
                    break;
                case BootMode.FlowFrom:
                    BootFlowFrom();
                    break;
            }
        }

        private void BootCombatOnly()
        {
            if (!TryResolveCombatManager()) return;

            _combatManager.SetCombatScene(_profile.combatScene);
            _combatManager.SetDebugOptions(_profile.skipBossIntro, _profile.startPhaseIndex);
            _combatManager.StartCombat();
        }

        private void BootFlowFrom()
        {
            if (!TryResolveFlowManager()) return;

            _flowManager.StartChapter(_profile.chapterId, _profile.step);
        }

        private bool TryResolveCombatManager()
        {
            if (_combatManager == null)
                _combatManager = FindAnyObjectByType<CombatManager>();

            if (_combatManager == null)
            {
                Debug.LogError("GameBootstrapper: CombatManager is not found in scene");
                return false;
            }
            return true;
        }

        private bool TryResolveFlowManager()
        {
            if (_flowManager == null)
                _flowManager = FindAnyObjectByType<FlowManager>();

            if (_flowManager == null)
            {
                Debug.LogError("GameBootstrapper: FlowManager is not found in scene");
                return false;
            }
            return true;
        }
    }
}
