using System;

namespace CodeBase.Data
{
  [Serializable]
  public class NotCollectedLoot
  {
    public PositionOnLevel PositionOnLevel;
    public Loot Loot;
    public string Id;

    public NotCollectedLoot(PositionOnLevel positionOnLevel, Loot loot, string id)
    {
      PositionOnLevel = positionOnLevel;
      Loot = loot;
      Id = id;
    }

  }
}