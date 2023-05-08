using System;
using System.Collections;
using CodeBase.Gameplay.Logic;
using UnityEngine;

namespace CodeBase.Gameplay.Enemy
{
  public class EnemyHealth : MonoBehaviour, IHealth
  {
    public EnemyAnimator Animator;

    [SerializeField]
    private float _current;

    [SerializeField]
    private float _max;

    private bool _canStun = true;

    public event Action HealthChanged;
    public event Action OnTakeDamage;

    public float Current
    {
      get => _current;
      set
      {
        if (_current == value)
          return;

        _current = value;
        HealthChanged?.Invoke();
      }
    }

    public float Max
    {
      get => _max;
      set => _max = value;
    }

    public float StunCooldown { get; set; }

    public void TakeDamage(float damage)
    {
      Current -= damage;

      if (_canStun)
        StunEnemy();
      OnTakeDamage?.Invoke();
    }

    private void StunEnemy()
    {
      Animator.PlayHit();
      StartCoroutine(MakeStunCooldown());
    }

    private IEnumerator MakeStunCooldown()
    {
      _canStun = false;
      yield return new WaitForSeconds(StunCooldown);
      _canStun = true;
    }
  }
}