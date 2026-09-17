using System;
using Project_Unorder.AgentSystem.BossSystem;
using UnityEngine;

namespace Project_Unorder.CombatSystem.Core
{
    public class CombatManager : MonoBehaviour, IEarlyAwakeableManager
    {
        public event Action OnCombatStartEvent;

        [SerializeField] private CombatScene _combatScene;
        [SerializeField] private BossDataSO _bossData;
        [SerializeField] private BossEncounterController _encounterController;
        [Header("Debug Options")]
        [SerializeField] private bool _skipIntro;
        [SerializeField] private int _startPhaseIndex;

        public CombatScene CurrentCombatScene => _combatScene;
        public BossDataSO CurrentBossData => _bossData;

        public void PreAwake()
        {
        }

        public void SetCombatScene(CombatScene combatScene)
        {
            _combatScene = combatScene;
            if (_combatScene != null)
                SetBossData(_combatScene.bossData);
        }

        public void SetBossData(BossDataSO data)
        {
            _bossData = data;
        }

        public void SetEncounterController(BossEncounterController controller)
        {
            _encounterController = controller;
        }

        public void SetDebugOptions(bool skipIntro, int startPhaseIndex)
        {
            _skipIntro = skipIntro;
            _startPhaseIndex = startPhaseIndex;
        }

        public bool StartCombat()
        {
            if (_encounterController == null)
                _encounterController = FindAnyObjectByType<BossEncounterController>();

            if (_encounterController == null)
            {
                Debug.LogError("CombatManager: BossEncounterController is not found in scene");
                return false;
            }

            OnCombatStartEvent?.Invoke();
            _encounterController.StartEncounter(_skipIntro, _startPhaseIndex);
            return true;
        }
    }
}
