using System;
using CodeBase.Data.Progress;
using CodeBase.Gameplay.Enemy;
using CodeBase.Gameplay.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.ProgressWatchers;
using CodeBase.StaticData.Monster;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Gameplay.Hero
{
  [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
  public class HeroAttack : MonoBehaviour, IProgressReader
  {
    public HeroAnimator Animator;
    public CharacterController CharacterController;

    [SerializeField]
    private GameObject _bloodFxPrefab;

    [SerializeField]
    private GameObject _rockHitFxPrefab;

    private IInputService _inputService;

    private static int _layerMask;
    private readonly Collider[] _hits = new Collider[3];
    private Stats _stats;

    public void Construct(IInputService inputService) =>
      _inputService = inputService;

    private void Awake() =>
      _layerMask = 1 << LayerMask.NameToLayer(Layers.HittableLayer);

    private void Update()
    {
      if (_inputService == null || Animator.IsAttacking)
        return;

      if (_inputService.IsFastAttackButtonUp())
        Animator.PlayFastAttack();

      if (_inputService.IsLongAttackButtonUp())
        Animator.PlayLongAttack();
    }

    private void OnFastAttack() => Attack(attackMultiplier: 1f);
    private void OnLongAttack() => Attack(attackMultiplier: 2f);

    private void Attack(float attackMultiplier)
    {
      //PhysicsDebug.DrawDebug(OverlapPosition(), _stats.DamageRadius, 100.0f);
      for (int i = 0; i < Hit(); ++i)
      {
        Transform enemyTransform = _hits[i].transform.parent;
        if (enemyTransform.TryGetComponent(out IHealth health))
        {
          health.TakeDamage(_stats.Damage * attackMultiplier);
          CreateHitFx(enemyTransform);
        }
      }
    }

    private void CreateHitFx(Transform enemyTransform)
    {
      MonsterTypeId monsterType = enemyTransform.GetComponent<EnemyType>().Value;
      switch (monsterType)
      {
        case MonsterTypeId.Lich:
          Instantiate(_bloodFxPrefab, enemyTransform.position + Vector3.up, Quaternion.identity);
          break;
        case MonsterTypeId.Golem:
          Instantiate(_rockHitFxPrefab, enemyTransform.position + Vector3.up, Quaternion.identity);
          break;
        case MonsterTypeId.FatDragon:
          Instantiate(_bloodFxPrefab, OverlapPosition(), Quaternion.identity);
          break;
      }
    }

    private int Hit() =>
      Physics.OverlapSphereNonAlloc(OverlapPosition(), _stats.DamageRadius, _hits, _layerMask);

    private Vector3 OverlapPosition() =>
      new Vector3(transform.position.x, CharacterController.center.y, transform.position.z) + transform.forward;

    public void ReceiveProgress(PlayerProgress progress) =>
      _stats = progress.HeroStats;
  }
}