using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.Shooter;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Patterns
{
    [CreateAssetMenu(menuName = "SO/Boss/Pattern/Wave")]
    public class WavePatternSO : BossAttackPatternSO
    {
        [SerializeField] private int _emitterIndex;
        [SerializeField] private int _waveBulletCount = 8;
        [SerializeField] private int _waveRepeat = 4;
        [SerializeField] private float _waveInterval = 0.2f;
        [SerializeField] private float _bulletSpeed = 7f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private float _waveAmplitude = 30f;
        [SerializeField] private float _baseAngle = 0f;

        public override IEnumerator Execute(BossAttackContext context)
        {
            BossProjectileEmitter emitter = context.Boss.GetCompo<BossProjectileEmitter>();

            for (int wave = 0; wave < _waveRepeat; wave++)
            {
                float phaseOffset = wave * (180f / _waveRepeat);
                for (int i = 0; i < _waveBulletCount; i++)
                {
                    float sineValue = Mathf.Sin((i / (float)_waveBulletCount) * Mathf.PI * 2f + phaseOffset * Mathf.Deg2Rad);
                    float angle = (_baseAngle + sineValue * _waveAmplitude) * Mathf.Deg2Rad;
                    Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    emitter.Emit(_emitterIndex, dir, _bulletSpeed, _damage, _lifeTime);
                }

                if (wave < _waveRepeat - 1)
                    yield return new WaitForSeconds(_waveInterval);
            }
        }
    }
}
