using System.Threading.Tasks;
using CodeBase.Gameplay.Enemy.Attack;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Gameplay.Enemy.Move;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Logic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories.Loot;
using CodeBase.Services.ProgressWatchers;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Monster;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factories.Enemy
{
  public class EnemyFactory : IEnemyFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IStaticDataService _staticData;
    private readonly HeroProvider _heroProvider;
    private readonly IRandomService _randomService;
    private readonly ILootFactory _lootFactory;

    public EnemyFactory(IAssetProvider assets, IProgressWatchers progressWatchers, IStaticDataService staticData, HeroProvider heroProvider, IRandomService randomService, ILootFactory lootFactory)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _staticData = staticData;
      _heroProvider = heroProvider;
      _randomService = randomService;
      _lootFactory = lootFactory;
    }

    public async Task<GameObject> CreateEnemy(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);
      GameObject heroGameObject = _heroProvider.HeroObject;

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monsterObject = Object.Instantiate(prefab, parent.position, parent.rotation, parent);
      _progressWatchers.Register(monsterObject);

      IHealth health = monsterObject.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monsterObject.GetComponent<ActorUI>().Construct(health);
      monsterObject.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      RotateOnDamage rotateOnDamage = monsterObject.GetComponent<RotateOnDamage>();
      rotateOnDamage.Construct(heroGameObject.transform);

      rotateOnDamage.Initialize(monsterData.RotationSpeed);
      
      EnemyAttack enemyAttack = monsterObject.GetComponent<EnemyAttack>();

      enemyAttack.Construct(heroGameObject.transform);
      enemyAttack.Damage = monsterData.Damage;
      enemyAttack.Cleavage = monsterData.Cleavage;
      enemyAttack.EffectiveDistance = monsterData.EffectiveDistance;
      enemyAttack.RotationSpeed = monsterData.RotationSpeed;

      monsterObject.GetComponent<AgentMoveToPlayer>().Construct(heroGameObject.transform);

      LootSpawner lootSpawner = monsterObject.GetComponentInChildren<LootSpawner>();
      lootSpawner.Construct(_lootFactory, _randomService);
      lootSpawner.SetLootValue(monsterData.MinLootValue, monsterData.MaxLootValue);

      return monsterObject;
    }
  }
}