using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Factories.Loot;
using CodeBase.Logic;
using CodeBase.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Gameplay.Enemy.Loot
{
  public class LootSpawner : MonoBehaviour
  {
    public EnemyDeath EnemyDeath;

    private IRandomService _randomizer;

    private int _minValue;
    private int _maxValue;
    private ILootFactory _lootFactory;

    public void Construct(ILootFactory lootFactory, IRandomService randomService)
    {
      _lootFactory = lootFactory;
      _randomizer = randomService;
    }
    
    private void Start()
    {
      EnemyDeath.Happened += SpawnLoot;
    }

    public void SetLootValue(int min, int max)
    {
      _minValue = min;
      _maxValue = max;
    }

    private async void SpawnLoot()
    {
      EnemyDeath.Happened -= SpawnLoot;

      LootPiece lootPiece = await _lootFactory.CreateLoot();
      lootPiece.transform.position = transform.position;
      lootPiece.GetComponent<UniqueId>().GenerateId();

      Data.Progress.Loot.Loot loot = GenerateLoot();
      
      lootPiece.Initialize(loot);
    }

    private Data.Progress.Loot.Loot GenerateLoot()
    {
      Data.Progress.Loot.Loot loot = new Data.Progress.Loot.Loot()
      {
        Value = _randomizer.Next(_minValue, _maxValue)
      };
      return loot;
    }
  }
}