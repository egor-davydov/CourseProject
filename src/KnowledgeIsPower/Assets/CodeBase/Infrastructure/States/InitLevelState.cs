using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Data.Progress.Loot;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Gameplay.Logic;
using CodeBase.Gameplay.Logic.EnemySpawners;
using CodeBase.Infrastructure.Factories.EnemySpawner;
using CodeBase.Infrastructure.Factories.LevelTransfer;
using CodeBase.Infrastructure.Factories.Loot;
using CodeBase.Infrastructure.Factories.SaveTrigger;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Level;
using CodeBase.UI.Services.Factory;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class InitLevelState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly IEnemySpawnerFactory _enemySpawnerFactory;
    private readonly ISaveTriggerFactory _saveTriggerFactory;
    private readonly ILevelTransferFactory _levelTransferFactory;
    private readonly ILootFactory _lootFactory;
    private readonly IUIFactory _uiFactory;
    private readonly IRespawnService _respawnService;

    public InitLevelState(
      GameStateMachine gameStateMachine,
      IStaticDataService staticData,
      IPersistentProgressService progressService,
      IEnemySpawnerFactory enemySpawnerFactory,
      ISaveTriggerFactory saveTriggerFactory,
      ILevelTransferFactory levelTransferFactory,
      ILootFactory lootFactory,
      IUIFactory uiFactory,
      IRespawnService respawnService
    )
    {
      _gameStateMachine = gameStateMachine;
      _staticData = staticData;
      _progressService = progressService;
      _enemySpawnerFactory = enemySpawnerFactory;
      _saveTriggerFactory = saveTriggerFactory;
      _levelTransferFactory = levelTransferFactory;
      _lootFactory = lootFactory;
      _uiFactory = uiFactory;
      _respawnService = respawnService;
    }

    public async void Enter()
    {
      LevelStaticData levelData = LevelStaticData();
      
      await InitSpawners(levelData);
      await InitSaveTriggers(levelData);
      await InitLevelTransfer(levelData);
      await InitUIRoot();
      await InitLootPieces();
      
      _gameStateMachine.Enter<InitHeroState>();
    }

    public void Exit()
    {
    }
    
    private async Task InitUIRoot() =>
      await _uiFactory.CreateUIRoot();

    private async Task InitSpawners(LevelStaticData levelStaticData)
    {
      List<SpawnPoint> spawners = new List<SpawnPoint>();
      foreach (EnemySpawnerStaticData spawnerData in levelStaticData.EnemySpawners)
      {
        SpawnPoint spawnPoint = await _enemySpawnerFactory
          .CreateSpawner(spawnerData.Id, spawnerData.TransformData, spawnerData.MonsterTypeId);
        spawners.Add(spawnPoint);
      }

      _respawnService.Initialize(spawners);
    }

    private async Task InitSaveTriggers(LevelStaticData levelStaticData)
    {
      foreach (SaveTriggerStaticData saveTriggerData in levelStaticData.SaveTriggers)
        await _saveTriggerFactory.
          CreateSaveTrigger(saveTriggerData.Id, saveTriggerData.TransformData, saveTriggerData.BoxColliderData);
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

    private async Task InitLevelTransfer(LevelStaticData levelData) =>
      await _levelTransferFactory.CreateLevelTransfer(levelData.LevelTransfer.Position);

    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);
  }
}