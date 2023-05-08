using System.Collections.Generic;
using CodeBase.Gameplay.Logic.EnemySpawners;
using UnityEngine;

namespace CodeBase.Services.LevelCleared
{
  public interface ILevelClearedService : IService
  {
    void InitializeSpawners(List<SpawnPoint> enemySpawners);
    void InitializeObjectToEnable(GameObject objectToEnable);
  }
}