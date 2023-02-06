using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
  public class EnemySpawner : MonoBehaviour, ISavedProgress
  {
    public MonsterTypeId MonsterTypeId;
    public bool Slayed;

    private string _id;

    private void Awake()
    {
      _id = GetComponent<UniqueId>().Id;
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(_id))
        Slayed = true;
      else
        Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (Slayed)
        progress.KillData.ClearedSpawners.Add(_id);
    }

    private void Spawn()
    {
    }
  }
}