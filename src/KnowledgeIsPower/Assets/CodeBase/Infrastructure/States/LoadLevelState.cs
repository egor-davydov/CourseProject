using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Data.Progress.Loot;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
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
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;
    private readonly IRespawnService _respawnService;

    public LoadLevelState(
      GameStateMachine gameStateMachine,
      IHeroStateMachine heroStateMachine,
      SceneLoader sceneLoader,
      LoadingCurtain loadingCurtain,
      IGameFactory gameFactory,
      IPersistentProgressService progressService,
      IStaticDataService staticDataService,
      IUIFactory uiFactory,
      IRespawnService respawnService
    )
    {
      _stateMachine = gameStateMachine;
      _heroStateMachine = heroStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticDataService;
      _uiFactory = uiFactory;
      _respawnService = respawnService;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _gameFactory.Cleanup();
      _gameFactory.WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _loadingCurtain.Hide();

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
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.ReceiveProgress(_progressService.Progress);
    }

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

      await InitSpawners(levelData);
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
        SpawnPoint spawnPoint = await _gameFactory.CreateSpawner(spawnerData.Id, spawnerData.TransformData, spawnerData.MonsterTypeId);
        spawners.Add(spawnPoint);
      }

      _respawnService.Initialize(spawners);
    }

    private async Task InitLootPieces()
    {
      foreach (KeyValuePair<string, LootPieceData> item in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary)
      {
        LootPiece lootPiece = await _gameFactory.CreateLoot();
        lootPiece.GetComponent<UniqueId>().Id = item.Key;
        lootPiece.Initialize(item.Value.Loot);
        lootPiece.transform.position = item.Value.Position.AsUnityVector();
      }
    }

    private async Task<GameObject> InitHero(LevelStaticData levelStaticData)
    {
      GameObject heroObject = await _gameFactory.CreateHero(levelStaticData.InitialHeroPosition);
      _heroStateMachine.Initialize(heroObject);

      return heroObject;
    }

    private async Task InitLevelTransfer(LevelStaticData levelData) =>
      await _gameFactory.CreateLevelTransfer(levelData.LevelTransfer.Position);

    private async Task InitHud(GameObject hero)
    {
      GameObject hud = await _gameFactory.CreateHud();

      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private void CameraFollow(GameObject hero) =>
      Camera.main.GetComponent<CameraFollow>().Follow(hero);
  }
}