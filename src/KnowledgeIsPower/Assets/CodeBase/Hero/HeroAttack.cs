using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
  public class HeroAttack : MonoBehaviour, ISavedProgressReader
  {
    public const int MaxEnemiesCountToHit = 3;

    public HeroAnimator HeroAnimator;
    public CharacterController CharacterController;
    private IInputService _input;
    private int _layerMask;
    private Collider[] _hits = new Collider[MaxEnemiesCountToHit];
    private Stats _stats;

    private void Awake()
    {
      _input = AllServices.Container.Single<IInputService>();

      _layerMask = 1 << LayerMask.NameToLayer("Hittable");
    }

    private void Update()
    {
      if (_input.IsAttackButtonUp() && !HeroAnimator.IsAttacking)
        HeroAnimator.PlayAttack();
    }

    private void OnAttack()
    {
      for (int i = 0; i < Hit(); i++)
      {
        _hits[i].transform.parent.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
      }
    }

    private int Hit() =>
      Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);

    private Vector3 StartPoint() =>
      new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);

    public void LoadProgress(PlayerProgress progress) =>
      _stats = progress.HeroStats;
  }
}