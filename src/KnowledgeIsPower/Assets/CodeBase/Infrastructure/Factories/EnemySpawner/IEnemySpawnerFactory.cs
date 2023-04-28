using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Gameplay.Logic.EnemySpawners;
using CodeBase.Services;
using CodeBase.StaticData.Monster;

namespace CodeBase.Infrastructure.Factories.EnemySpawner
{
  public interface IEnemySpawnerFactory : IService
  {
    Task<SpawnPoint> CreateSpawner(string spawnerId, TransformData transformData, MonsterTypeId monsterTypeId);
  }
}