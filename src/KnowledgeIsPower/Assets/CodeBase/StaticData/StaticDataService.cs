using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string EnemiesStaticDataPath = "StaticData/Monsters";
    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

    public void LoadMonsters()
    {
      _monsters = Resources
        .LoadAll<MonsterStaticData>(EnemiesStaticDataPath)
        .ToDictionary(x=>x.TypeId, x=> x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
      _monsters.TryGetValue(typeId, out MonsterStaticData monsterData)
        ? monsterData
        : null;
  }
}