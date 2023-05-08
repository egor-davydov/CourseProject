using System.Collections.Generic;
using CodeBase.Gameplay.Logic.EnemySpawners;

namespace CodeBase.Services.Respawn
{
  public class RespawnService : IRespawnService
  {
    private List<SpawnPoint> _enemySpawners;

    public void Initialize(List<SpawnPoint> enemySpawners) =>
      _enemySpawners = enemySpawners;

    public void RespawnEnemies()
    {
      foreach (SpawnPoint spawnPoint in _enemySpawners)
      {
        if (spawnPoint.Slain) 
          spawnPoint.Spawn();
      }
    }
  }
}