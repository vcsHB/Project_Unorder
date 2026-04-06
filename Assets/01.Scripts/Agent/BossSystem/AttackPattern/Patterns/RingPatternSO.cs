using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.Shooter;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Patterns
{
    [CreateAssetMenu(menuName = "SO/Boss/Pattern/Ring")]
    public class RingPatternSO : BossAttackPatternSO
    {
        [SerializeField] private int _emitterIndex;
        [SerializeField] private int _bulletCount = 12;
        [SerializeField] private float _bulletSpeed = 6f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private int _ringCount = 1;
        [SerializeField] private float _ringInterval = 0.3f;
        [SerializeField] private float _rotationOffsetPerRing = 15f;

        public override IEnumerator Execute(BossAttackContext context)
        {
            BossProjectileEmitter emitter = context.Boss.GetCompo<BossProjectileEmitter>();
            float angleStep = 360f / _bulletCount;

            for (int ring = 0; ring < _ringCount; ring++)
            {
                float offset = ring * _rotationOffsetPerRing;
                for (int i = 0; i < _bulletCount; i++)
                {
                    float angle = (angleStep * i + offset) * Mathf.Deg2Rad;
                    Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    emitter.Emit(_emitterIndex, dir, _bulletSpeed, _damage, _lifeTime);
                }

                if (ring < _ringCount - 1)
                    yield return new WaitForSeconds(_ringInterval);
            }
        }
    }
}
