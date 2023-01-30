using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator), typeof(EnemyHealth))]
  public class EnemyDeath : MonoBehaviour
  {
    public float DestroyTime = 3f;

    public EnemyHealth Health;
    public GameObject DeathFx;
    public EnemyAnimator Animator;

    public event Action Happend;

    private void Start() =>
      Health.HealthChanged += HealthChanged;

    private void OnDestroy() =>
      Health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
      if (Health.Current <= 0)
        Die();
    }
    
    private void Die()
    {
      Health.HealthChanged -= HealthChanged;

      Animator.PlayDeath();
      SpawnDeathFx();
      StartCoroutine(DestroyTimer());

      Happend?.Invoke();
    }

    private void SpawnDeathFx() => 
      Instantiate(DeathFx, transform.position, Quaternion.identity);

    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(DestroyTime);
      Destroy(gameObject);
    }
  }
}