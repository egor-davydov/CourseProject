using System.Linq;
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
    public float EffectiveDistance = 0.5f;
    public float Cleavage = 0.5f;

    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private bool _isAttacking;
    private float _attackCooldown;
    private Collider[] _hits = new Collider[1];
    private int _layerMask;

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
        
      }
    }

    private bool Hit(out Collider hit)
    {
      int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();
      
      return hitsCount >= 1;
    }

    private Vector3 StartPoint() => 
      new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

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