using System;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroHealth))]
  public class HeroDeath : MonoBehaviour
  {
    public HeroHealth Health;
    public HeroMove Move;
    public GameObject DeathFx;
    public HeroAnimator Animator;
    
    private bool _isDead;

    private void Start() => 
      Health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
      Health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
      if (!_isDead && Health.Current <= 0)
        Die();
    }

    private void Die()
    {
      _isDead = true;
      
      Move.enabled = false;
      Animator.PlayDeath();
      Instantiate(DeathFx, transform.position, Quaternion.identity);
    }
  }
}