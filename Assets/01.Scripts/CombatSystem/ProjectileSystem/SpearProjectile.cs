using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public class SpearProjectile : SelectableProjectile
    {
        private Animator _animator;
        [SerializeField] private float _delayTime = 1f;
        [SerializeField] private string _delayAnimationName;
        private int _delayAnimationHash;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            _delayAnimationHash = Animator.StringToHash(_delayAnimationName);

        }


        public override void Shoot(ProjectileData projectileData)
        {
            _visualTrm.up = -projectileData.direction;
            _animator.SetTrigger(_delayAnimationHash);
            base.Shoot(projectileData);
            Invoke(nameof(SetVelocity), _delayTime);
        }

        private void SetVelocity()
        {
            _rigidCompo.linearVelocity = _data.direction.normalized * _data.speed;

        }


    }
}