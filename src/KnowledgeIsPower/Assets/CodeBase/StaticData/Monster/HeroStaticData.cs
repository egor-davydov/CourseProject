using UnityEngine;

namespace CodeBase.StaticData.Monster
{
  [CreateAssetMenu(fileName = "HeroData", menuName = "Static Data/Hero")]
  public class HeroStaticData : ScriptableObject
  {
    [Range(1, 100)]
    public int MaxHp = 50;

    [Range(1, 30)]
    public float Damage = 1;

    [Range(.5f, 1)]
    public float DamageRadius = 0.5f;

    [Range(1, 30)]
    public float MaxDamageToCompleteBlock = 10f;

    [Range(1, 30)]
    public float DefendFactor = 0.5f;

    [Range(1, 30)]
    public float BasicMovementSpeed = 3f;

    [Range(1, 30)]
    public float FocusedMovementSpeed = 2f;

    [Range(1, 30)]
    public float RotationSpeed = 10f;
  }
}