﻿using UnityEngine;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
  public class MonsterStaticData : ScriptableObject
  {
    [Range(1,100)]
    public int Hp;
    
    [Range(1,50)]
    public float Damage;
    
    [Range(0.5f,1)]
    public float Cleavage;
    [Range(0.5f,1)]
    public float EffectiveDistance;

    public GameObject prefab;
  }
}