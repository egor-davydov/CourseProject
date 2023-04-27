using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Data.Progress.Loot;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Gameplay.Logic;
using CodeBase.Gameplay.Logic.EnemySpawners;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories.EnemySpawner;
using CodeBase.Infrastructure.Factories.Hero;
using CodeBase.Infrastructure.Factories.Hud;
using CodeBase.Infrastructure.Factories.LevelTransfer;
using CodeBase.Infrastructure.Factories.Loot;
using CodeBase.Infrastructure.Factories.SaveTrigger;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Level;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private readonly GameStateMachine _stateMachine;
    private readonly IHeroStateMachine _heroStateMachine;
    private readonly HeroProvider _heroProvider;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IAssetProvider _assets;
    private readonly IPersistentProgressService _progressService;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IStaticDataService _staticData;
    private readonly IEnemySpawnerFactory _enemySpawnerFactory;
    private readonly ISaveTriggerFactory _saveTriggerFactory;
    private readonly ILootFactory _lootFactory;
    private readonly IHeroFactory _heroFactory;
    private readonly IHudFactory _hudFactory;
    private readonly ILevelTransferFactory _levelTransferFactory;
    private readonly IUIFactory _uiFactory;
    private readonly IRespawnService _respawnService;

    public LoadLevelState(
      GameStateMachine gameStateMachine,
      IHeroStateMachine heroStateMachine,
      HeroProvider heroProvider,
      SceneLoader sceneLoader,
      LoadingCurtain loadingCurtain,
      IAssetProvider assets,
      IPersistentProgressService progressService,
      IProgressWatchers progressWatchers,
      IStaticDataService staticDataService,
      IEnemySpawnerFactory enemySpawnerFactory,
      ISaveTriggerFactory saveTriggerFactory,
      ILootFactory lootFactory,
      IHeroFactory heroFactory,
      IHudFactory hudFactory,
      ILevelTransferFactory levelTransferFactory,
      IUIFactory uiFactory,
      IRespawnService respawnService
    )
    {
      _stateMachine = gameStateMachine;
      _heroStateMachine = heroStateMachine;
      _heroProvider = heroProvider;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _assets = assets;
      _progressService = progressService;
      _progressWatchers = progressWatchers;
      _staticData = staticDataService;
      _enemySpawnerFactory = enemySpawnerFactory;
      _saveTriggerFactory = saveTriggerFactory;
      _lootFactory = lootFactory;
      _heroFactory = heroFactory;
      _hudFactory = hudFactory;
      _levelTransferFactory = levelTransferFactory;
      _uiFactory = uiFactory;
      _respawnService = respawnService;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      Cleanup();
      WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _loadingCurtain.Hide();

    private async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.Loot);
      await _assets.Load<GameObject>(AssetAddress.Spawner);
    }

    private void Cleanup()
    {
      _progressWatchers.Readers.Clear();
      _progressWatchers.Writers.Clear();

      _assets.Cleanup();
    }


    private async void OnLoaded()
    {
      await InitUIRoot();
      await InitGameWorld();
      InformProgressReaders();

      _stateMachine.Enter<GameLoopState>();
    }

    private async Task InitUIRoot() =>
      await _uiFactory.CreateUIRoot();

    private void InformProgressReaders()
    {
      foreach (IProgressReader progressReader in _progressWatchers.Readers)
        progressReader.ReceiveProgress(_progressService.Progress);
    }

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

      await InitSpawners(levelData);
      await InitSaveTriggers(levelData);
      await InitLootPieces();
      GameObject hero = await InitHero(levelData);
      await InitLevelTransfer(levelData);
      await InitHud(hero);
      CameraFollow(hero);
    }

    private async Task InitSpawners(LevelStaticData levelStaticData)
    {
      List<SpawnPoint> spawners = new List<SpawnPoint>();
      foreach (EnemySpawnerStaticData spawnerData in levelStaticData.EnemySpawners)
      {
        SpawnPoint spawnPoint = await _enemySpawnerFactory.CreateSpawner(spawnerData.Id, spawnerData.TransformData, spawnerData.MonsterTypeId);
        spawners.Add(spawnPoint);
      }

      _respawnService.Initialize(spawners);
    }

    private async Task InitSaveTriggers(LevelStaticData levelStaticData)
    {
      foreach (SaveTriggerStaticData saveTriggerData in levelStaticData.SaveTriggers)
        await _saveTriggerFactory.CreateSaveTrigger(saveTriggerData.Id, saveTriggerData.TransformData, saveTriggerData.BoxColliderData);
    }

    private async Task InitLootPieces()
    {
      foreach (KeyValuePair<string, LootPieceData> item in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary)
      {
        LootPiece lootPiece = await _lootFactory.CreateLoot();
        lootPiece.GetComponent<UniqueId>().Id = item.Key;
        lootPiece.Initialize(item.Value.Loot);
        lootPiece.transform.position = item.Value.Position.AsUnityVector();
      }
    }

    private async Task<GameObject> InitHero(LevelStaticData levelStaticData)
    {
      GameObject heroObject = await _heroFactory.CreateHero(levelStaticData.InitialHeroPosition);
      _heroStateMachine.Initialize(heroObject);
      _heroProvider.Initialize(heroObject);
      _heroStateMachine.Enter(HeroStateType.Basic);
      
      return heroObject;
    }

    private async Task InitLevelTransfer(LevelStaticData levelData) =>
      await _levelTransferFactory.CreateLevelTransfer(levelData.LevelTransfer.Position);

    private async Task InitHud(GameObject hero)
    {
      GameObject hud = await _hudFactory.CreateHud();

      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private void CameraFollow(GameObject hero) =>
      Camera.main.GetComponent<CameraFollow>().Follow(hero);
  }
}