using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public class DummyShooterObject : MonoBehaviour
    {
        [SerializeField] private Transform _testTargetTrm;

        [SerializeField] private RepeatShooter _repeatShooter;


        private void Awake()
        {
            Debug.Assert(_repeatShooter);
            _repeatShooter.SetTarget(_testTargetTrm);
        }
    }
}