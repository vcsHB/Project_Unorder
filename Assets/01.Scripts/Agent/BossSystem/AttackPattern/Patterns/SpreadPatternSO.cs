using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.Shooter;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Patterns
{
    [CreateAssetMenu(menuName = "SO/Boss/Pattern/Spread")]
    public class SpreadPatternSO : BossAttackPatternSO
    {
        [SerializeField] private int _emitterIndex;
        [SerializeField] private int _bulletCount = 5;
        [SerializeField] private float _spreadAngle = 60f;
        [SerializeField] private float _bulletSpeed = 8f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifeTime = 4f;

        public override IEnumerator Execute(BossAttackContext context)
        {
            BossProjectileEmitter emitter = context.Boss.GetCompo<BossProjectileEmitter>();
            Vector2 toPlayer = (context.PlayerTarget.position - context.Boss.transform.position).normalized;
            float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
            float halfSpread = _spreadAngle * 0.5f;
            float step = _bulletCount > 1 ? _spreadAngle / (_bulletCount - 1) : 0f;

            for (int i = 0; i < _bulletCount; i++)
            {
                float angle = (baseAngle - halfSpread + step * i) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                emitter.Emit(_emitterIndex, dir, _bulletSpeed, _damage, _lifeTime);
            }

            yield break;
        }
    }
}
