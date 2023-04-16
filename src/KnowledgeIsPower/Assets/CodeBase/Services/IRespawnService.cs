using System.Collections.Generic;
using CodeBase.Logic.EnemySpawners;

namespace CodeBase.Services
{
  public interface IRespawnService : IService
  {
    void Initialize(List<SpawnPoint> enemySpawners);
    void RespawnEnemies();
  }
}