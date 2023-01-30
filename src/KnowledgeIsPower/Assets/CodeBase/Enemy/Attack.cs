﻿using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class Attack : MonoBehaviour
  {
    public EnemyAnimator EnemyAnimator;

    public float AttackCooldown = 3f;
    public float EffectiveDistance = 0.5f;
    public float Cleavage = 0.5f;
    public float Damage = 10f;

    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private bool _isAttacking;
    private float _attackCooldown;
    private Collider[] _hits = new Collider[1];
    private int _layerMask;
    private bool _attackIsActive;

    private void Awake()
    {
      _gameFactory = AllServices.Container.Single<IGameFactory>();

      _layerMask = 1 << LayerMask.NameToLayer("Player");

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
      if (Hit(out Collider hit))
      {
        PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1);
        hit.GetComponent<IHealth>().TakeDamage(Damage);
      }
    }

    private void OnAttackEnded()
    {
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }

    public void EnableAttack() =>
      _attackIsActive = true;

    public void DisableAttack() =>
      _attackIsActive = false;

    private bool Hit(out Collider hit)
    {
      int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();

      return hitsCount >= 1;
    }

    private Vector3 StartPoint() =>
      new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

    private void UpdateCooldown()
    {
      if (!CooldownIsUp())
        _attackCooldown -= Time.deltaTime;
    }

    private bool CanAttack() =>
      _attackIsActive && !_isAttacking && CooldownIsUp();

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