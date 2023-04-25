using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Gameplay.Enemy.Attack;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Gameplay.Enemy.Move;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private readonly IAssetProvider _assets;
    private readonly IInputService _inputService;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _persistentProgressService;
    private GameObject _heroGameObject;
    private readonly IWindowService _windowService;
    private readonly IGameStateMachine _stateMachine;
    private readonly IHeroStateMachine _heroStateMachine;

    public GameFactory(
      IAssetProvider assets,
      IInputService inputService,
      IStaticDataService staticData,
      IRandomService randomService,
      IPersistentProgressService persistentProgressService,
      IWindowService windowService,
      IGameStateMachine stateMachine,
      IHeroStateMachine heroStateMachine
      )
    {
      _assets = assets;
      _inputService = inputService;
      _staticData = staticData;
      _randomService = randomService;
      _persistentProgressService = persistentProgressService;
      _windowService = windowService;
      _stateMachine = stateMachine;
      _heroStateMachine = heroStateMachine;
    }

    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.Loot);
      await _assets.Load<GameObject>(AssetAddress.Spawner);
    }

    public async Task<GameObject> CreateHero(Vector3 at)
    {
      _heroGameObject = await InstantiateRegisteredAsync(AssetAddress.HeroPath, at);
      _heroGameObject.GetComponent<HeroAnimator>().Construct(_heroStateMachine);
      _heroGameObject.GetComponent<HeroMove>().Construct(_inputService, _heroStateMachine);
      _heroGameObject.GetComponent<HeroDefend>().Construct(_inputService);
      return _heroGameObject;
    }

    public async Task CreateLevelTransfer(Vector3 at)
    {
      GameObject prefab = await InstantiateRegisteredAsync(AssetAddress.LevelTransferTrigger, at);
      LevelTransferTrigger levelTransfer = prefab.GetComponent<LevelTransferTrigger>();

      levelTransfer.Construct(_stateMachine);
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);

      hud.GetComponentInChildren<FocusOnEnemyButton>()
        .Initialize(_heroStateMachine, _heroGameObject.GetComponentInChildren<FocusSphere>());
      hud.GetComponentInChildren<LootCounter>()
        .Construct(_persistentProgressService.Progress.WorldData);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
        openWindowButton.Init(_windowService);

      return hud;
    }

    public async Task<LootPiece> CreateLoot()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
      LootPiece lootPiece = InstantiateRegistered(prefab)
        .GetComponent<LootPiece>();

      lootPiece.Construct(_persistentProgressService.Progress.WorldData);

      return lootPiece;
    }

    public async Task<GameObject> CreateEnemy(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monster = Object.Instantiate(prefab, parent.position, parent.rotation, parent);

      IHealth health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      Attack attack = monster.GetComponent<Attack>();
      attack.Construct(_heroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;

      monster.GetComponent<AgentMoveToPlayer>()?.Construct(_heroGameObject.transform);
      monster.GetComponent<RotateToHero>()?.Construct(_heroGameObject.transform);

      LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.Construct(this, _randomService);
      lootSpawner.SetLootValue(monsterData.MinLootValue, monsterData.MaxLootValue);

      return monster;
    }

    public async Task<SpawnPoint> CreateSpawner(string spawnerId, TransformData transform, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);

      SpawnPoint spawner = InstantiateRegistered(prefab, transform.position.AsUnityVector(), transform.rotation).GetComponent<SpawnPoint>();

      spawner.Construct(this);
      spawner.MonsterTypeId = monsterTypeId;
      spawner.Id = spawnerId;
      return spawner;
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();

      _assets.Cleanup();
    }

    private GameObject InstantiateRegistered(GameObject prefab, Vector3 asUnityVector, Quaternion asUnityQuaternion)
    {
      GameObject gameObject = Object.Instantiate(prefab, asUnityVector, asUnityQuaternion);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private GameObject InstantiateRegistered(GameObject prefab)
    {
      GameObject gameObject = Object.Instantiate(prefab);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
    {
      GameObject gameObject = await _assets.Instantiate(path: prefabPath, at: at);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
      GameObject gameObject = await _assets.Instantiate(path: prefabPath);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }
  }
}