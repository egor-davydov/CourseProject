using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Enemy;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroFocusOnEnemy : MonoBehaviour
  {
    private Transform EnemyToFocus
    {
      get => _enemyToFocus;
      set
      {
        if (_enemyToFocus != null)
          _enemyToFocus.GetComponent<EnemyFocused>().UnFocus();

        _enemyToFocus = value;
        if (_enemyToFocus != null) _enemyToFocus.GetComponent<EnemyFocused>().Focus();
      }
    }

    private List<Transform> _enemiesInSphere;
    private int _currentFocusedEnemyNumber;
    private Transform _enemyToFocus;

    public void Initialize()
    {
      FocusSphere focusSphere = GetComponentInChildren<FocusSphere>();

      _enemiesInSphere = focusSphere.EnemiesInSphere;
      EnemyToFocus = _enemiesInSphere[0];
      _currentFocusedEnemyNumber = 0;
    }

    private void Update()
    {
      if (EnemyToFocus != null)
        LookAt(transform, EnemyToFocus);
    }

    private static void LookAt(Transform thisTransform, Transform target)
    {
      Vector3 viewForward = target.position - thisTransform.position;
      viewForward.Normalize();

      Vector3 viewUp = Vector3.up - Vector3.Project(viewForward, Vector3.up);
      viewUp.Normalize();

      thisTransform.forward = new Vector3(viewForward.x, 0, viewForward.z);
    }

    public void ChangeEnemyToFocusLeft()
    {
      int enemyNumber = _currentFocusedEnemyNumber == 0
        ? _enemiesInSphere.Count - 1
        : _currentFocusedEnemyNumber - 1;

      EnemyToFocus = _enemiesInSphere[enemyNumber];
      _currentFocusedEnemyNumber = enemyNumber;
    }

    public void ChangeEnemyToFocusRight()
    {
      int enemyNumber = _currentFocusedEnemyNumber == _enemiesInSphere.Count - 1
        ? 0
        : _currentFocusedEnemyNumber + 1;

      EnemyToFocus = _enemiesInSphere[enemyNumber];
      _currentFocusedEnemyNumber = enemyNumber;
    }

    public void UnFocus() =>
      EnemyToFocus = null;
  }
}