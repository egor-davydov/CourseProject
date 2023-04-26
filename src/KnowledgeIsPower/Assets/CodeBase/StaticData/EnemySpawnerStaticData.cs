using System;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class EnemySpawnerStaticData
  {
    public string Id;
    public MonsterTypeId MonsterTypeId;
    public TransformData TransformData;

    public EnemySpawnerStaticData(string id, MonsterTypeId monsterTypeId, TransformData transformData)
    {
      Id = id;
      MonsterTypeId = monsterTypeId;
      TransformData = transformData;
    }
  }
}