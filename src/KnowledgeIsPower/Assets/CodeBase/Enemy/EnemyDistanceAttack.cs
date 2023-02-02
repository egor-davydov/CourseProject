using CodeBase.Enemy.Fireball;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class EnemyDistanceAttack : EnemyAttack
  {
    public float AttackCooldown = 1.5f;
    public float ProjectileSpawnDistance = 0.5f;
    public float Damage = 10f;
    public float ShootingHeight = 1f;

    private bool _isAttacking;
    private float _attackCooldown;
    private FireballHit _fireball;

    private void Update()
    {
      UpdateCooldown();

      if (CanAttack())
        StartAttack();
    }
    
    private void OnAttack()
    {
      GameObject fireball = GameFactory.CreateFireball(StartPoint());
      Vector3 heroPosition = HeroTransform.position;
      fireball.transform.LookAt(new Vector3(heroPosition.x, heroPosition.y + ShootingHeight, heroPosition.z));
      _fireball = fireball.GetComponent<FireballHit>();
      _fireball.Happened += MakeHit;
    }

    private void OnAttackEnded()
    {
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }

    private void MakeHit(Collider collider) => 
      collider.GetComponent<IHealth>().TakeDamage(Damage);
    
    private Vector3 StartPoint() =>
      new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * ProjectileSpawnDistance;

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