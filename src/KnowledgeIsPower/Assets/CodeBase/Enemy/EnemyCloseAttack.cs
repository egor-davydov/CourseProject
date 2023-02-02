using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class EnemyCloseAttack :  EnemyAttack
  {
    public float EffectiveDistance = 0.5f;
    public float Cleavage = 0.5f;
    public float AttackCooldown = 3f;
    public float Damage = 10f;

    private Collider[] _hits = new Collider[1];
    private int _layerMask;
    private bool _isAttacking;
    private float _attackCooldown;

    private void Awake() => 
      _layerMask = 1 << LayerMask.NameToLayer("Player");

    private void Update()
    {
      UpdateCooldown();

      if (CanAttack())
        StartAttack();
    }

    private void OnAttack()
    {
      if (Hit(out Collider hit))
      {
        PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1);
        MakeHit(hit);
      }
    }

    private void OnAttackEnded()
    {
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }

    private bool Hit(out Collider hit)
    {
      int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();

      return hitsCount >= 1;
    }

    private void MakeHit(Collider collider) => 
      collider.GetComponent<IHealth>().TakeDamage(Damage);

    private Vector3 StartPoint() =>
      new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

    private void UpdateCooldown()
    {
      if (!CooldownIsUp())
        _attackCooldown -= Time.deltaTime;
    }

    private bool CanAttack() =>
      AttackIsActive && !_isAttacking && CooldownIsUp();

    private bool CooldownIsUp() =>
      _attackCooldown <= 0;

    private void StartAttack()
    {
      transform.LookAt(HeroTransform);
      EnemyAnimator.PlayAttack();

      _isAttacking = true;
    }
  }
}