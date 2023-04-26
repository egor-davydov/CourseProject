using UnityEngine;

namespace CodeBase.Gameplay.Enemy.Attack
{
  [RequireComponent(typeof(Attack))]
  public class CheckAttackRange : MonoBehaviour
  {
    public Attack Attack;
    public TriggerObserver TriggerObserver;

    private void Start()
    {
      TriggerObserver.TriggerEnter += TriggerEnter;
      TriggerObserver.TriggerExit += TriggerExit;
      
      Attack.DisableAttack();
    }

    private void OnDestroy()
    {
      TriggerObserver.TriggerEnter -= TriggerEnter;
      TriggerObserver.TriggerExit -= TriggerExit;
    }

    private void TriggerExit(Collider obj) => 
      Attack.DisableAttack();

    private void TriggerEnter(Collider obj) => 
      Attack.EnableAttack();
  }
}