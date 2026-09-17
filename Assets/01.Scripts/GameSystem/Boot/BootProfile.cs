using Project_Unorder.CombatSystem.Core;
using Project_Unorder.Core.Attribute;
using UnityEngine;

namespace Project_Unorder.BootSystem
{
    [CreateAssetMenu(menuName = "SO/Boot/BootProfile")]
    public class BootProfile : ScriptableObject
    {
        public BootMode mode = BootMode.Normal;

        [Condition(nameof(mode), (int)BootMode.CombatOnly)]
        public CombatScene combatScene;
        [Condition(nameof(mode), (int)BootMode.CombatOnly)]
        public int startPhaseIndex;

        [Condition(nameof(mode), (int)BootMode.FlowFrom)]
        public uint chapterId;
        [Condition(nameof(mode), (int)BootMode.FlowFrom)]
        public uint step;

        public bool skipBossIntro;

        public bool OverridesSceneFlow => mode != BootMode.Normal;
    }
}
