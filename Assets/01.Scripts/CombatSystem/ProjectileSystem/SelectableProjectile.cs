using Project_Unorder.AgentSystem.InteractSystem;
using UnityEngine;

namespace Project_Unorder.CombatSystem.ProjectileSystem
{
    public class SelectableProjectile : Projectile, ISelectable
    {
        [SerializeField] private SelectVisual _selectVisual;

        private float _cachedSpeed;
        private Vector2 _cachedVelocity;
        private bool _isSelected;

        public virtual void Select()
        {
            if (_isSelected) return;

            _isSelected = true;
            _cachedSpeed = _data.speed;
            _cachedVelocity = _rigidCompo.linearVelocity;

            _data.speed = 0f;
            _rigidCompo.linearVelocity = Vector2.zero;

            _selectVisual.Select();
        }

        public virtual void Release()
        {
            if (!_isSelected) return;

            _isSelected = false;
            _data.speed = _cachedSpeed;
            _rigidCompo.linearVelocity = _cachedVelocity;

            _selectVisual.Release();
        }
    }
}
