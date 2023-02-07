using UnityEngine;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "HeroData", menuName = "StaticData/Hero")]
  public class HeroStaticData : ScriptableObject
  {
    [Range(1, 100)] public int Hp;
    [Range(1, 50)] public float Damage;
    [Range(0.5f, 1)] public float DamageRadius;
    [Range(1f, 50)] public float MovementSpeed;
  }
}