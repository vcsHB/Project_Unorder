using System;
using UnityEngine;
namespace Project_Unorder.RuleSystem
{
    public static class RuleObserver
    {
        public static class RULE_BoxLimit
        {
            public static void Trigger(bool isViolated)
            {
                OnRuleTriggerEvent?.Invoke(isViolated);
            }

            public static event Action<bool> OnRuleTriggerEvent; // parameter : Violation
        }

        public static class RULE_LifeTime
        {
            public static void Trigger(bool isViolated)
            {
                OnRuleTriggerEvent?.Invoke(isViolated);
            }

            public static event Action<bool> OnRuleTriggerEvent; // parameter : Violation
        }
    }
}