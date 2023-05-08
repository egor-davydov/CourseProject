using System.Collections.Generic;
using CodeBase.Data.Progress;
using CodeBase.Extensions.GameplayExtensions;
using CodeBase.Gameplay.Enemy;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroFocusOnEnemy : MonoBehaviour, IProgressReader
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
    private float _rotationSpeed;

    public void Initialize()
    {
      _enemiesInSphere = GetComponentInChildren<HeroFocusSphere>().EnemiesInSphere;
      FocusOnEnemyFromSphere(enemyNumber: 0);
    }

    private void Update()
    {
      if (CurrentEnemyToFocus != null)
        transform.SmoothLookAt(CurrentEnemyToFocus, _rotationSpeed * Time.deltaTime);
    }

    public void ReceiveProgress(PlayerProgress progress) =>
      _rotationSpeed = progress.HeroStats.RotationSpeed;

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

    public void EnemyLeftFromSphere()
    {
      if (!HeroUnFocused() && _enemiesInSphere.Count != 0)
        ChangeEnemyToFocusRight();
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

    private bool HeroUnFocused() =>
      _currentEnemyToFocus == null;
  }
}