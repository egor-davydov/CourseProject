using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Gameplay.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
  public class SpawnPoint : MonoBehaviour, ISavedProgress
  {
    public MonsterTypeId MonsterTypeId;
    
    public string Id { get; set; }

    private IGameFactory _factory;
    
    private EnemyDeath _enemyDeath;

    public bool Slain { get; private set; }

    public void Construct(IGameFactory gameFactory) => 
      _factory = gameFactory;

    private void OnDestroy()
    {
      if (_enemyDeath != null)
        _enemyDeath.Happened -= Slay;
    }

    public void ReceiveProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(Id))
        Slain = true;
      else
        Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      List<string> slainSpawnersList = progress.KillData.ClearedSpawners;
      
      if(Slain && !slainSpawnersList.Contains(Id))
        slainSpawnersList.Add(Id);
    }

    public async void Spawn()
    {
      GameObject monster = await _factory.CreateEnemy(MonsterTypeId, transform);
      _enemyDeath = monster.GetComponent<EnemyDeath>();
      _enemyDeath.Happened += Slay;
    }

    private void Slay()
    {
      if (_enemyDeath != null)
        _enemyDeath.Happened -= Slay;
      
      Slain = true;
    }
  }
}