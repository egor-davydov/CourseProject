using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Hero
{
  public class HeroDeath : MonoBehaviour
  {
    public HeroHealth Health;
    
    public HeroMove Move;
    public HeroAttack Attack;
    public HeroAnimator Animator;

    public GameObject DeathFx;
    private bool _isDead;
    private IWindowService _windowService;

    public void Construct(IWindowService windowService)
    {
      _windowService = windowService;
    }

    private void Start()
    {
      Health.HealthChanged += HealthChanged;
    }

    private void OnDestroy()
    {
      Health.HealthChanged -= HealthChanged;
    }

    public void MakeAlive()
    {
      _isDead = false;
      Move.enabled = true;
      Attack.enabled = true;
      Health.Current = Health.Max;
      Animator.ResetToIdle();
    }
    
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
      Instantiate(DeathFx, transform.position, Quaternion.identity);
      _windowService.Open(WindowId.HeroDeath);
    }
  }
}