using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public WorldData WorldData;
    public State HeroState;

    public PlayerProgress(string initialLevel, float maxHP)
    {
      WorldData = new WorldData(initialLevel);
      HeroState = new State();
      HeroState.MaxHP = maxHP;
      HeroState.ResetHP();
    }
  }

  [Serializable]
  public class State
  {
    public float CurrentHP;
    public float MaxHP;
    public void ResetHP() => CurrentHP = MaxHP;
  }
}