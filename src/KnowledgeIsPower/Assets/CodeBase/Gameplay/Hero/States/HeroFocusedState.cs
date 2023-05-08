using UnityEngine;

namespace CodeBase.Gameplay.Hero.States
{
  public class HeroFocusedState : IHeroState
  {
    private GameObject _heroObject;
    private HeroFocusOnEnemy _heroFocusOnEnemy;

    public void Initialize(GameObject heroObject) =>
      _heroObject = heroObject;

    public void Enter()
    {
      Debug.Log($"{Time.time} HeroFocusedState");
      _heroObject.GetComponent<HeroAnimator>().TurnOffRun();
      _heroFocusOnEnemy = _heroObject.GetComponent<HeroFocusOnEnemy>();
      _heroFocusOnEnemy.Initialize();
    }

    public void Exit() =>
      _heroFocusOnEnemy.UnFocus();
  }
}