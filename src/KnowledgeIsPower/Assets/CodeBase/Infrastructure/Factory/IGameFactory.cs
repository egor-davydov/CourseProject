using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    Task<GameObject> CreateHero(Vector3 at);
    Task<GameObject> CreateHud();
    Task<GameObject> CreateEnemy(MonsterTypeId typeId, Transform parent);
    Task<LootPiece> CreateLoot();
    Task<SpawnPoint> CreateSpawner(string spawnerId, TransformData transform, MonsterTypeId monsterTypeId);
    void Cleanup();
    Task WarmUp();
    Task CreateLevelTransfer(Vector3 at);
  }
}