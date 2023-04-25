using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero.States
{
  public class HeroFocusedState : IHeroState, IUpdatable
  {
    private GameObject _heroObject;
    private Collider _firstEnemy;

    public void Initialize(GameObject heroObject) =>
      _heroObject = heroObject;

    public void Enter()
    {
      FocusSphere focusSphere = _heroObject.GetComponentInChildren<FocusSphere>();

      List<Collider> enemiesInSphere = focusSphere.EnemiesInSphere;
      _firstEnemy = enemiesInSphere[0];
    }

    public void OnUpdate()
    {
      if (_firstEnemy != null)
        _heroObject.transform.LookAt(_firstEnemy.transform);
    }
  }
}