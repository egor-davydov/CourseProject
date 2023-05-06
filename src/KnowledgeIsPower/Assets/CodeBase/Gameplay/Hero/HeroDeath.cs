using CodeBase.Gameplay.Logic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroDeath : MonoBehaviour
  {
    public HeroHealth Health;
    
    public HeroMove Move;
    public HeroAttack Attack;
    public HeroAnimator Animator;

    public GameObject DeathFx;
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
      Attack.enabled = false;
      Animator.PlayDeath();
      Animator.StateExited += CreateDeathFx;

      void CreateDeathFx(AnimatorState animatorState) => 
        Instantiate(DeathFx, transform.position, Quaternion.identity);
    }
  }
}