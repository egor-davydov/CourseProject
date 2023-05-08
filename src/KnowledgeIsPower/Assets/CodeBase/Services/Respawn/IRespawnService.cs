using System.Collections.Generic;
using CodeBase.Gameplay.Logic.EnemySpawners;

namespace CodeBase.Services.Respawn
{
  public interface IRespawnService : IService
  {
    void Initialize(List<SpawnPoint> enemySpawners);
    void RespawnEnemies();
  }
}