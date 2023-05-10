using System;
using CodeBase.Data.Progress;
using CodeBase.Gameplay.Logic;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroHealth : MonoBehaviour, IHealth, IProgressWriter, IProgressReader
  {
    [SerializeField]
    private AudioSource _heroAudioSource;
    [SerializeField]
    private AudioClip _hitSound;
    [SerializeField]
    private HeroAnimator Animator;

    [SerializeField]
    private HeroDefend HeroDefend;

    private State _state;
    private bool _defending;

    public event Action HealthChanged;
    public event Action OnTakeDamage;

    public float Current
    {
      get => _state.CurrentHP;
      set
      {
        if (value == _state.CurrentHP)
          return;

        _state.CurrentHP = value;
        HealthChanged?.Invoke();
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
      if (!HeroDefend.IsActive)
      {
        finalDamage = damage;
      }
      else
      {
        if (HeroDefend.MaxDamageToCompleteBlock > damage)
          finalDamage = 0;
        else
          finalDamage = damage - damage * HeroDefend.DefendFactor;
      }


      if (finalDamage != 0)
      {
        Current -= finalDamage;
        Animator.PlayHit(); 
        _heroAudioSource.PlayOneShot(_hitSound);
      }

      OnTakeDamage?.Invoke();
      //Debug.Log($"Damage {finalDamage}");
    }
  }
}