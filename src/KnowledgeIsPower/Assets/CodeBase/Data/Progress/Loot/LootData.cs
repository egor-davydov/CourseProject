using System;

namespace CodeBase.Data.Progress.Loot
{
  [Serializable]
  public class LootData
  {
    public int Collected;
    public LootPiecesOnLevelsDictionary LootPiecesOnLevels = new LootPiecesOnLevelsDictionary();
    public Action Changed;

    public void Collect(Loot loot)
    {
      Collected += loot.Value;
      Changed?.Invoke();
    }

    public void Add(int lootValue)
    {
      Collected += lootValue;
      Changed?.Invoke();
    }
  }
}