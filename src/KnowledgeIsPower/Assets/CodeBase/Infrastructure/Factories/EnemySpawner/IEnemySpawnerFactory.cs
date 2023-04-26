using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.Factories.EnemySpawner
{
  public interface IEnemySpawnerFactory : IService
  {
    Task<SpawnPoint> CreateSpawner(string spawnerId, TransformData transform, MonsterTypeId monsterTypeId);
  }
}