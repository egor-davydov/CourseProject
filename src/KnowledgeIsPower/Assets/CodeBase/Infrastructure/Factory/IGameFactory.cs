using System.Collections.Generic;
using System.Threading.Tasks;
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
    Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent);
    Task<LootPiece> CreateLoot();
    Task<SpawnPoint> CreateSpawner(string spawnerId, Vector3 at, MonsterTypeId monsterTypeId);
    void Cleanup();
    Task WarmUp();
    Task CreateLevelTransfer(Vector3 at);
  }
}