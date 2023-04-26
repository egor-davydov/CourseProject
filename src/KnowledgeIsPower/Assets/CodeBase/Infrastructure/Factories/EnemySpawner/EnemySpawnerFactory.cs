using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories.Enemy;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.ProgressWatchers;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.EnemySpawner
{
  public class EnemySpawnerFactory : IEnemySpawnerFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IEnemyFactory _enemyFactory;

    public EnemySpawnerFactory(IAssetProvider assets, IProgressWatchers progressWatchers, IEnemyFactory enemyFactory)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _enemyFactory = enemyFactory;
    }

    public async Task<SpawnPoint> CreateSpawner(string spawnerId, TransformData transform, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);

      GameObject spawnerObject = Object.Instantiate(prefab, transform.position.AsUnityVector(), transform.rotation);
      _progressWatchers.Register(spawnerObject);
      SpawnPoint spawner = spawnerObject.GetComponent<SpawnPoint>();
      spawner.Construct(_enemyFactory);
      spawner.MonsterTypeId = monsterTypeId;
      spawner.Id = spawnerId;
      return spawner;
    }
  }
}