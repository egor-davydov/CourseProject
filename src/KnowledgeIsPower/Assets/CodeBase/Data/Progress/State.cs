using System;

namespace CodeBase.Data.Progress
{
  [Serializable]
  public class State
  {
    public float CurrentHP;
    public float MaxHP;

    public void ResetHP()
    {
      CurrentHP = MaxHP;
    }
  }
}