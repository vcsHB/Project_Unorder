using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.Shooter;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Patterns
{
    [CreateAssetMenu(menuName = "SO/Boss/Pattern/Aimed")]
    public class AimedPatternSO : BossAttackPatternSO
    {
        [SerializeField] private int _emitterIndex;
        [SerializeField] private int _burstCount = 3;
        [SerializeField] private float _burstInterval = 0.15f;
        [SerializeField] private float _bulletSpeed = 10f;
        [SerializeField] private float _damage = 15f;
        [SerializeField] private float _lifeTime = 4f;
        [SerializeField] private bool _isHoming;

        public override IEnumerator Execute(BossAttackContext context)
        {
            BossProjectileEmitter emitter = context.Boss.GetCompo<BossProjectileEmitter>();

            for (int i = 0; i < _burstCount; i++)
            {
                Vector2 toPlayer = (context.PlayerTarget.position - context.Boss.transform.position).normalized;
                Transform homingTarget = _isHoming ? context.PlayerTarget : null;
                emitter.Emit(_emitterIndex, toPlayer, _bulletSpeed, _damage, _lifeTime, homingTarget);

                if (i < _burstCount - 1)
                    yield return new WaitForSeconds(_burstInterval);
            }
        }
    }
}
