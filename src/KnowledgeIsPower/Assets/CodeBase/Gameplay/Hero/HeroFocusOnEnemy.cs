using System.Collections.Generic;
using CodeBase.Gameplay.Enemy;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroFocusOnEnemy : MonoBehaviour
  {
    private Transform CurrentEnemyToFocus
    {
      get => _currentEnemyToFocus;
      set
      {
        if (HasFocus())
          UnFocus();

        Focus(value);
      }
    }

    private List<Transform> _enemiesInSphere;
    private int _currentFocusedEnemyNumber;
    private Transform _currentEnemyToFocus;

    public void Initialize()
    {
      _enemiesInSphere = GetComponentInChildren<FocusSphere>().EnemiesInSphere;
      FocusOnEnemyFromSphere(enemyNumber: 0);
    }

    private void Update()
    {
      if (CurrentEnemyToFocus != null)
        LookAt(transform, CurrentEnemyToFocus);
    }

    public void ChangeEnemyToFocusLeft()
    {
      int enemyNumber = _currentFocusedEnemyNumber == 0
        ? _enemiesInSphere.Count - 1
        : _currentFocusedEnemyNumber - 1;

      FocusOnEnemyFromSphere(enemyNumber);
    }

    public void ChangeEnemyToFocusRight()
    {
      int enemyNumber = _currentFocusedEnemyNumber == _enemiesInSphere.Count - 1
        ? 0
        : _currentFocusedEnemyNumber + 1;

      FocusOnEnemyFromSphere(enemyNumber);
    }

    public void UnFocus()
    {
      _currentEnemyToFocus.GetComponent<EnemyForFocus>().UnFocus();
      _currentEnemyToFocus = null;
    }

    private void Focus(Transform enemyForFocus)
    {
      _currentEnemyToFocus = enemyForFocus;
      _currentEnemyToFocus.GetComponent<EnemyForFocus>().Focus();
    }

    private bool HasFocus() =>
      _currentEnemyToFocus != null;

    private void FocusOnEnemyFromSphere(int enemyNumber)
    {
      CurrentEnemyToFocus = _enemiesInSphere[enemyNumber];
      _currentFocusedEnemyNumber = enemyNumber;
    }

    private void LookAt(Transform thisTransform, Transform target)
    {
      Vector3 viewForward = target.position - thisTransform.position;
      viewForward.Normalize();

      Vector3 viewUp = Vector3.up - Vector3.Project(viewForward, Vector3.up);
      viewUp.Normalize();

      thisTransform.forward = new Vector3(viewForward.x, 0, viewForward.z);
    }
  }
}