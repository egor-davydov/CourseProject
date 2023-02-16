using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
  [Serializable]
  public class LootData
  {
    public int Collected;
    public Action Changed;
    public List<NotCollectedLoot> NotCollectedLoot = new List<NotCollectedLoot>();

    public void Collect(Loot loot)
    {
      Collected += loot.Value;
      Changed?.Invoke();
    }
  }
}