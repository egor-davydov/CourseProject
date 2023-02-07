using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string EnemiesStaticDataPath = "StaticData/Monsters";
    private const string HeroStaticDataPath = "StaticData/Hero";
    
    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

    public HeroStaticData HeroData { get; set; }

    public void LoadMonsters() =>
      _monsters = Resources
        .LoadAll<MonsterStaticData>(EnemiesStaticDataPath)
        .ToDictionary(x=>x.TypeId, x=> x);

    public void LoadHero() => 
      HeroData = Resources.Load<HeroStaticData>(HeroStaticDataPath);

    public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
      _monsters.TryGetValue(typeId, out MonsterStaticData monsterData)
        ? monsterData
        : null;
  }
}