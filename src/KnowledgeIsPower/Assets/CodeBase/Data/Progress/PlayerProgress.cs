using System;

namespace CodeBase.Data.Progress
{
  [Serializable]
  public class PlayerProgress
  {
    public State HeroState;
    public WorldData WorldData;
    public Stats HeroStats;
    public KillData KillData;
    public PurchaseData PurchaseData;


    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      PurchaseData = new PurchaseData();
      HeroState = new State();
      HeroStats = new Stats();
      KillData = new KillData();
    }
  }
}