using System;
using CodeBase.Data;
using CodeBase.StaticData.Monster;

namespace CodeBase.StaticData.Level
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