﻿using System.Collections.Generic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
  public class LevelStaticData : ScriptableObject
  {
    public string LevelKey;
    public List<EnemySpawnerData> EnemySpawners;
  }
}