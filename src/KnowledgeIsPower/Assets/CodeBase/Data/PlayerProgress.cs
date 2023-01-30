using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public WorldData WorldData;
    public State HeroState;
    public Stats HeroStats;

    public PlayerProgress(string initialLevel, float maxHP, float damage, float damageRadius)
    {
      WorldData = new WorldData(initialLevel);
      HeroState = new State();
      HeroStats = new Stats();
      
      HeroState.MaxHP = maxHP;
      HeroState.ResetHP();
      HeroStats.Damage = damage;
      HeroStats.DamageRadius = damageRadius;
    }
  }
}