using Project_Unorder.CombatSystem;
using UnityEngine;
namespace Project_Unorder.ObjectManage
{

    public class BoxWall : MonoBehaviour
    {
        private Collider2D _collider;
        [SerializeField] private SpriteRenderer _wallRenderer;
        [SerializeField] private bool _allowCollision;


        public void SetCollider(bool value)
        {
            _collider.enabled = value;
        }

        public void SwitchCollisionMode(bool allowCollidion)
        {
            _allowCollision = allowCollidion;
            _collider.isTrigger = allowCollidion;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_allowCollision)
            {
                if (collision.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(new DamageData()
                    {
                        damage = 500f
                    });
                }
            }
        }
    }
}