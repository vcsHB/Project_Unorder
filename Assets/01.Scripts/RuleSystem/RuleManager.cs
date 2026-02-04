using UnityEngine;
namespace Project_Unorder.RuleSystem
{
    // Observer -> Manager -> Penalty
    public class RuleManager : MonoBehaviour, IEarlyAwakeableManager
    {
        public void PreAwake()
        {   
            //RuleObserver.RULE_BoxLimit.OnRuleTriggerEvent += Handle;
        }

        private void Awake()
        {
            
        }
    }
}