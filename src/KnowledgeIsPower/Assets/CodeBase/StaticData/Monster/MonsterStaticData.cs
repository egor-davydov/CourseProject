﻿using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Monster
{
  [CreateAssetMenu(fileName = "MonsterData", menuName = "Static Data/Monster")]
  public class MonsterStaticData : ScriptableObject
  {
    public MonsterTypeId MonsterTypeId;
    
    [Range(1,100)]
    public int Hp = 50;
    
    [Range(1,30)]
    public float Damage = 10;

    public int MaxLootValue = 10;
    public int MinLootValue = 0;

    [Range(.5f,1)]
    public float EffectiveDistance = .5f;
    
    [Range(.5f,1)]
    public float Cleavage = .5f;

    [Range(0,10)]
    public float MoveSpeed = 3;
    
    [Range(0.1f,15)]
    public float RotationSpeed = 3;
    
    public AssetReferenceGameObject PrefabReference;
  }
}