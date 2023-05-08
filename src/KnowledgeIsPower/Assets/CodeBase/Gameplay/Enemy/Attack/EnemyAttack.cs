using System.Collections;
using System.Linq;
using CodeBase.Extensions.GameplayExtensions;
using CodeBase.Gameplay.Logic;
using CodeBase.StaticData.Monster;
using UnityEngine;

namespace CodeBase.Gameplay.Enemy.Attack
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class EnemyAttack : MonoBehaviour
  {
    [SerializeField]
    private EnemyAnimator _enemyAnimator;

    public float AttackCooldown = 3.0f;
    public float Cleavage = 0.5f;
    public float EffectiveDistance = 0.5f;
    public float Damage = 10;
    public float RotationSpeed;

    private Transform _heroTransform;
    private Collider[] _hits = new Collider[1];
    private int _layerMask;
    private float _attackCooldown;
    private bool _isAttacking;
    private bool _attackIsActive;

    [SerializeField]
    private GameObject _orangeHitFxPrefab;

    [SerializeField]
    private GameObject _whiteHitFxPrefab;

    private MonsterTypeId _monsterType;


    public void Construct(Transform heroTransform) =>
      _heroTransform = heroTransform;

    private void Awake()
    {
      _layerMask = 1 << LayerMask.NameToLayer(Layers.PlayerLayer);
      _monsterType = GetComponent<EnemyType>().Value;
    }

    private void Update()
    {
      UpdateCooldown();

      if (_attackIsActive)
        LookAtHero();

      if (CanAttack())
        StartAttack();
    }

    private void LookAtHero() =>
      transform.SmoothLookAt(_heroTransform, RotationSpeed * Time.deltaTime);

    private void OnAttack()
    {
      if (Hit(out Collider hit))
      {
        //PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1.0f);
        hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
        CreateHitFx(hit.transform);
      }
    }

    public void DisableAttack() =>
      _attackIsActive = false;

    public void EnableAttack() =>
      _attackIsActive = true;

    private void CreateHitFx(Transform heroTransform)
    {
      Vector3 heroPosition = heroTransform.position + Vector3.up;
      switch (_monsterType)
      {
        case MonsterTypeId.Lich:
          Instantiate(_whiteHitFxPrefab, heroPosition, Quaternion.identity);
          break;
        case MonsterTypeId.Golem:
          Instantiate(_whiteHitFxPrefab, heroPosition, Quaternion.identity);
          break;
        case MonsterTypeId.FatDragon:
          Instantiate(_orangeHitFxPrefab, heroPosition, Quaternion.identity);
          break;
      }
    }

    private bool CooldownIsUp() =>
      _attackCooldown <= 0f;

    private void UpdateCooldown()
    {
      if (!CooldownIsUp())
        _attackCooldown -= Time.deltaTime;
    }

    private bool Hit(out Collider hit)
    {
      var hitAmount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();

      return hitAmount > 0;
    }

    private Vector3 StartPoint()
    {
      return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
             transform.forward * EffectiveDistance;
    }

    private bool CanAttack() =>
      _attackIsActive && !_isAttacking && CooldownIsUp();

    private void StartAttack()
    {
      _enemyAnimator.PlayAttack();
      _isAttacking = true;
      StartCoroutine(ActivateAttack());
    }

    private IEnumerator ActivateAttack()
    {
      yield return new WaitForSeconds(1);
      EndAttack();
    }

    private void EndAttack()
    {
      _isAttacking = false;
      _attackCooldown = AttackCooldown;
    }
  }
}