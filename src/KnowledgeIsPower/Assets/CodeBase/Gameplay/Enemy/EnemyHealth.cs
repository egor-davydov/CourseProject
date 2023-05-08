using System;
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

    public event Action HealthChanged;
    public event Action OnTakeDamage;

    public float Current
    {
      get => _current;
      set
      {
        if(_current == value)
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

    public void TakeDamage(float damage)
    {
      Current -= damage;
      
      Animator.PlayHit();
      OnTakeDamage?.Invoke();
    }
  }
}