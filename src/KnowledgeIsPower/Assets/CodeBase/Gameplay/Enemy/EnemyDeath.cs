using System;
using System.Collections;
using CodeBase.Gameplay.Enemy.Attack;
using CodeBase.Gameplay.Enemy.Move;
using UnityEngine;

namespace CodeBase.Gameplay.Enemy
{
  [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
  public class EnemyDeath : MonoBehaviour
  {
    [SerializeField]
    private EnemyHealth Health;

    [SerializeField]
    private EnemyAnimator Animator;

    [SerializeField]
    private GameObject DeathFx;

    public event Action Happened;
    public event Action ObjectDestroyed;

    private void Start() =>
      Health.HealthChanged += OnHealthChanged;

    private void OnDestroy() =>
      Health.HealthChanged -= OnHealthChanged;

    private void OnDeathEnd()
    {
      SpawnDeathFx();
      StartCoroutine(DestroyEnemy());
    }

    private IEnumerator DestroyEnemy()
    {
      yield return new WaitForSeconds(2);
      Destroy(gameObject);
      ObjectDestroyed?.Invoke();
    }

    private void OnHealthChanged()
    {
      if (Health.Current <= 0)
        Die();
    }

    private void Die()
    {
      Health.HealthChanged -= OnHealthChanged;

      DisableScripts();
      //DestroyHurtBox();
      DestroyCanvas();
      Animator.PlayDeath();

      Happened?.Invoke();
    }

    private void DestroyCanvas() => 
      Destroy(GetComponentInChildren<Canvas>().gameObject);

    private void DestroyHurtBox() =>
      Destroy(GetComponentInChildren<BoxCollider>());

    private void DisableScripts()
    {
      GetComponent<EnemyAttack>().enabled = false;
      GetComponent<Aggro>().enabled = false;
      GetComponent<AgentMoveToPlayer>().enabled = false;
    }

    private void SpawnDeathFx() =>
      Instantiate(DeathFx, transform.position, Quaternion.identity);
  }
}