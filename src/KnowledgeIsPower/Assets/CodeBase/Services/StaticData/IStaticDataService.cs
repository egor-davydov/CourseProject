﻿using System.Collections.Generic;
using CodeBase.StaticData;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Monster;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
    LevelStaticData ForLevel(string sceneKey);
    WindowConfig ForWindow(WindowId shop);
    Dictionary<string, LevelStaticData> Levels { get; }
    HeroStaticData ForHero();
  }
}