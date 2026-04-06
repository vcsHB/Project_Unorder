using System.Collections;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.Cinematic
{
    public abstract class BossCinematic : ScriptableObject
    {
        public abstract IEnumerator Play(MonoBehaviour runner, Boss boss);
    }
}
