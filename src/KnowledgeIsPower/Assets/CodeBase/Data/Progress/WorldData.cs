using System;
using CodeBase.Data.Progress.Loot;

namespace CodeBase.Data.Progress
{
  [Serializable]
  public class WorldData
  {
    public PositionOnLevel PositionOnLevel;
    public LootData LootData;

    public WorldData(string initialLevel)
    {
      PositionOnLevel = new PositionOnLevel(initialLevel);
      LootData = new LootData();
    }
  }
}