using System;
using CodeBase.Data.Progress;
using CodeBase.Gameplay.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroHealth : MonoBehaviour, IHealth, IProgressWriter, IProgressReader
  {
    [SerializeField]
    private HeroAnimator Animator;

    [SerializeField]
    private HeroDefend HeroDefend;

    private State _state;
    private bool _defending;

    public event Action HealthChanged;

    private void Start()
    {
      HeroDefend.Activate += ActivateDefending;
      HeroDefend.Deactivate += DeactivateDefending;
    }

    private void OnDestroy()
    {
      HeroDefend.Activate -= ActivateDefending;
      HeroDefend.Deactivate -= DeactivateDefending;
    }

    public float Current
    {
      get => _state.CurrentHP;
      set
      {
        if (value != _state.CurrentHP)
        {
          _state.CurrentHP = value;

          HealthChanged?.Invoke();
        }
      }
    }

    public float Max
    {
      get => _state.MaxHP;
      set => _state.MaxHP = value;
    }


    public void ReceiveProgress(PlayerProgress progress)
    {
      _state = progress.HeroState;
      HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.HeroState.CurrentHP = Current;
      progress.HeroState.MaxHP = Max;
    }

    public void TakeDamage(float damage)
    {
      if (Current <= 0)
        return;

      float finalDamage;
      if (!_defending)
      {
        finalDamage = damage;
      }
      else
      {
        if (HeroDefend.MaximumDamageToBlock > damage)
          finalDamage = 0;
        else
          finalDamage = damage - damage * HeroDefend.DefendFactor;
      }

      Current -= finalDamage;

      Animator.PlayHit();
      //Debug.Log($"Damage {finalDamage}");
    }

    private void ActivateDefending() =>
      _defending = true;

    private void DeactivateDefending() =>
      _defending = false;
  }
}