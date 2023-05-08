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

    private void OnDeathEnd() =>
      CreateDeathFx();

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
    }

    private void CreateDeathFx() =>
      Instantiate(DeathFx, transform.position + transform.forward * 0.5f, Quaternion.identity);
  }
}