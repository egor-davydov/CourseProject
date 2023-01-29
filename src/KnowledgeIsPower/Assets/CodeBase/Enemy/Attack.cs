using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class Attack : MonoBehaviour
  {
    public EnemyAnimator EnemyAnimator;

    public float AttackCooldown = 3f;

    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private bool _isAttacking;
    private float _attackCooldown;

    private void Awake()
    {
      _gameFactory = AllServices.Container.Single<IGameFactory>();
      _gameFactory.HeroCreated += OnHeroCreated;
    }

    private void Update()
    {
      UpdateCooldown();

      if (CanAttack())
        StartAttack();
    }

    private void OnAttack()
    {
    }

    private void OnAttackEnded()
    {
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }

    private void UpdateCooldown()
    {
      if (!CooldownIsUp())
        _attackCooldown -= Time.deltaTime;
    }

    private bool CanAttack() =>
      !_isAttacking && CooldownIsUp();

    private bool CooldownIsUp() =>
      _attackCooldown <= 0;

    private void StartAttack()
    {
      transform.LookAt(_heroTransform);
      EnemyAnimator.PlayAttack();

      _isAttacking = true;
    }

    private void OnHeroCreated() =>
      _heroTransform = _gameFactory.HeroGameObject.transform;
  }
}