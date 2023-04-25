using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero.States
{
  public class HeroFocusedState : IHeroState, IUpdatable
  {
    private GameObject _heroObject;
    private Transform _enemyToFocus;

    public void Initialize(GameObject heroObject) =>
      _heroObject = heroObject;

    public void Enter()
    {
      _heroObject.GetComponent<HeroAnimator>().TurnOffRun();
      
      FocusSphere focusSphere = _heroObject.GetComponentInChildren<FocusSphere>();

      List<Transform> enemiesInSphere = focusSphere.EnemiesInSphere;
      _enemyToFocus = enemiesInSphere[0];
      _heroObject.transform.LookAt(_enemyToFocus.transform);
    }

    public void Exit() => 
      _enemyToFocus = null;

    public void OnUpdate()
    {
      if (_enemyToFocus != null)
        _heroObject.transform.LookAt(_enemyToFocus);
    }
  }
}