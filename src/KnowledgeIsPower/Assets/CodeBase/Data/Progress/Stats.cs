using System;

namespace CodeBase.Data.Progress
{
  [Serializable]
  public class Stats
  {
    public float Damage;
    public float DamageRadius;
    public float MaxDamageToCompleteBlock;
    public float DefendFactor;
    public float BasicMovementSpeed;
    public float FocusedMovementSpeed;
    public float RotationSpeed;
  }
}