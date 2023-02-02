using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  public abstract class EnemyAttack : MonoBehaviour
  {
    public EnemyAnimator EnemyAnimator;

    protected IGameFactory GameFactory;
    protected Transform HeroTransform;
    protected bool AttackIsActive;

    private void Awake()
    {
      GameFactory = AllServices.Container.Single<IGameFactory>();

      GameFactory.HeroCreated += OnHeroCreated;
    }

    public void EnableAttack() =>
      AttackIsActive = true;

    public void DisableAttack() =>
      AttackIsActive = false;

    private void OnHeroCreated() =>
      HeroTransform = GameFactory.HeroGameObject.transform;
  }
}