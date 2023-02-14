using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Random;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class LootSpawner : MonoBehaviour
  {
    public EnemyDeath EnemyDeath;
    private IGameFactory _factory;
    private int _lootMax;
    private int _lootMin;
    private IRandomService _random;

    public void Construct(GameFactory factory, IRandomService random)
    {
      _factory = factory;
      _random = random;
    }

    private void Start() => 
      EnemyDeath.Happened += SpawnLoot;

    public void SetLoot(int min, int max)
    {
      _lootMin = min;
      _lootMax = max;
    }
    private void SpawnLoot()
    {
      GameObject loot = _factory.CreateLoot();
      loot.transform.position = transform.position;

      var lootItem = new Loot
      {
        Value = _random.Next(_lootMin, _lootMax)
      };
    }
  }
}