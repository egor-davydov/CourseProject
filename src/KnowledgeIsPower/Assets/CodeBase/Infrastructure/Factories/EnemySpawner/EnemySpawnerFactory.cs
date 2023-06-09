﻿using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.Logic.EnemySpawners;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories.Enemy;
using CodeBase.Services.ProgressWatchers;
using CodeBase.StaticData.Monster;
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

    public async Task<SpawnPoint> CreateSpawner(string spawnerId, TransformData transformData, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
      GameObject spawnerObject = Object.Instantiate(prefab, transformData.Position.AsUnityVector(), transformData.Rotation.AsUnityQuaternion());
      _progressWatchers.Register(spawnerObject);
      
      SpawnPoint spawner = spawnerObject.GetComponent<SpawnPoint>();
      spawner.Construct(_enemyFactory);
      spawner.MonsterTypeId = monsterTypeId;
      spawner.Id = spawnerId;
      return spawner;
    }
  }
}