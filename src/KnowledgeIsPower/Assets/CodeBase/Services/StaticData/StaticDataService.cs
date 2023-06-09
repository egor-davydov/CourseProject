﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Monster;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string MonstersDataPath = "Static Data/Monsters";
    private const string LevelsDataPath = "Static Data/Levels";
    private const string StaticDataWindowPath = "Static Data/UI/WindowStaticData";
    private const string HeroDataPath = "Static Data/Hero/Hero";

    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;
    private HeroStaticData _hero;

    public Dictionary<string, LevelStaticData> Levels => _levels;

    public void Load()
    {
      _monsters = Resources
        .LoadAll<MonsterStaticData>(MonstersDataPath)
        .ToDictionary(x => x.MonsterTypeId, x => x);

      _levels = Resources
        .LoadAll<LevelStaticData>(LevelsDataPath)
        .ToDictionary(x => x.LevelKey, x => x);

      _hero = Resources
        .Load<HeroStaticData>(HeroDataPath);
      _windowConfigs = Resources
        .Load<WindowStaticData>(StaticDataWindowPath)
        .Configs
        .ToDictionary(x => x.WindowId, x => x);
    }
    
    public HeroStaticData ForHero() => _hero;

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
      _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
        ? staticData
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData
        : null;

    public WindowConfig ForWindow(WindowId windowId) =>
      _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
        ? windowConfig
        : null;
  }
}