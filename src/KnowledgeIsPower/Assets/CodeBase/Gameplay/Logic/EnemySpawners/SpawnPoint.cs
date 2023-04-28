using System.Collections.Generic;
using CodeBase.Data.Progress;
using CodeBase.Gameplay.Enemy;
using CodeBase.Infrastructure.Factories.Enemy;
using CodeBase.Services.ProgressWatchers;
using CodeBase.StaticData.Monster;
using UnityEngine;

namespace CodeBase.Gameplay.Logic.EnemySpawners
{
  public class SpawnPoint : MonoBehaviour, IProgressWriter, IProgressReader
  {
    public MonsterTypeId MonsterTypeId;

    public string Id { get; set; }

    private IEnemyFactory _enemyFactory;

    private EnemyDeath _enemyDeath;

    public bool Slain { get; private set; }

    public void Construct(IEnemyFactory enemyFactory) =>
      _enemyFactory = enemyFactory;

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

      if (Slain && !slainSpawnersList.Contains(Id))
        slainSpawnersList.Add(Id);
    }

    public async void Spawn()
    {
      GameObject monster = await _enemyFactory.CreateEnemy(MonsterTypeId, transform);
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