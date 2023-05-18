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

    [SerializeField]
    private AudioSource _heroAudioSource;
    [SerializeField]
    private AudioClip _fallAudio;
    private bool _isDead;

    private void Start() =>
      Health.HealthChanged += HealthChanged;

    private void OnDestroy() =>
      Health.HealthChanged -= HealthChanged;

    private void OnDeathEnd()
    {
      CreateDeathFx();
      PlayFallSound();
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
    }

    private void CreateDeathFx() =>
      Instantiate(DeathFx, transform.position + transform.forward * 0.5f, Quaternion.identity);

    private void PlayFallSound() => 
      _heroAudioSource.PlayOneShot(_fallAudio);
  }
}