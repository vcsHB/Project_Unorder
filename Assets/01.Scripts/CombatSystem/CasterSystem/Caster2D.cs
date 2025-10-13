using UnityEngine;
namespace Project_Unorder.CombatSystem.CasterSystem
{

    public abstract class Caster2D : Caster
    {
        protected Collider2D[] _hitBodys;
        protected ICastable2D[] _subCasters;

       

        protected override void Initialize()
        {
            base.Initialize();
            _hitBodys = new Collider2D[_detectMaxTargetAmount];
            _subCasters = GetComponents<ICastable2D>();
        }

        public void ForceCast(Collider2D[] hit)
        {
            for (int i = 0; i < hit.Length; i++)
                ForceCast(hit[i]);
        }

        public void ForceCast(Collider2D hit)
        {
            for (int j = 0; j < _subCasters.Length; j++)
                _subCasters[j].Cast(hit);

            OnCastSuccessEvent?.Invoke();
        }
    }
}