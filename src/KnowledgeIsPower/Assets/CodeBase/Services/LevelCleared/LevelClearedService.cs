using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Logic.EnemySpawners;
using UnityEngine;

namespace CodeBase.Services.LevelCleared
{
  public class LevelClearedService : ILevelClearedService
  {
    private GameObject _objectToEnable;
    private List<SpawnPoint> _enemySpawners;
    
    public void InitializeSpawners(List<SpawnPoint> enemySpawners)
    {
      _enemySpawners = enemySpawners;
      foreach (SpawnPoint enemySpawner in _enemySpawners)
        enemySpawner.OnSlain += OnEnemyKilled;
    }

    public void InitializeObjectToEnable(GameObject objectToEnable) =>
      _objectToEnable = objectToEnable;

    private void OnEnemyKilled()
    {
      if (_enemySpawners.All(spawnPoint => spawnPoint.Slain))
        LevelCleared();
    }

    private void LevelCleared() =>
      _objectToEnable.SetActive(true);
  }
}