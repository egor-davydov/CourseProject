using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroFocusOnEnemy : MonoBehaviour
  {
    private Transform _enemyToFocus;
    private List<Transform> _enemiesInSphere;
    private int _currentFocusedEnemyNumber;

    public void Initialize()
    {
      FocusSphere focusSphere = GetComponentInChildren<FocusSphere>();

      _enemiesInSphere = focusSphere.EnemiesInSphere;
      _enemyToFocus = _enemiesInSphere[0];
      _currentFocusedEnemyNumber = 0;
    }

    private void Update()
    {
      if (_enemyToFocus != null) 
        LookAt(transform, _enemyToFocus);
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
      Debug.Log($"_enemiesInSphere.Count {_enemiesInSphere.Count}");
      _enemyToFocus = _currentFocusedEnemyNumber == 0
        ? _enemiesInSphere[_enemiesInSphere.Count - 1]
        : _enemiesInSphere[_currentFocusedEnemyNumber - 1];
    }

    public void ChangeEnemyToFocusRight()
    {
      Debug.Log($"_enemiesInSphere.Count {_enemiesInSphere.Count}");
      _enemyToFocus = _currentFocusedEnemyNumber == _enemiesInSphere.Count - 1
        ? _enemiesInSphere[0]
        : _enemiesInSphere[_currentFocusedEnemyNumber + 1];
    }

    public void UnFocus() =>
      _enemyToFocus = null;
  }
}