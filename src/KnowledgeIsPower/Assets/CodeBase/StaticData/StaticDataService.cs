using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string EnemiesStaticDataPath = "Enemies/StaticData";
    private Dictionary<MonsterTypeId, GameObject> _monsters;

    public void LoadData()
    {
      _monsters = Resources.LoadAll<MonsterStaticData>(EnemiesStaticDataPath)
        .ToDictionary(x=>x.TypeId, x=> x.Prefab);
    }

    public GameObject ForMonster(MonsterTypeId typeId) => 
      _monsters.TryGetValue(typeId, out GameObject monsterPrefab)
        ? monsterPrefab
        : null;
  }
}