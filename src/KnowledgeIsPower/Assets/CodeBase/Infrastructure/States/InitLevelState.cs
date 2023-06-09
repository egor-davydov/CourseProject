﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Data.Progress.Loot;
using CodeBase.Extensions;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Gameplay.Logic;
using CodeBase.Gameplay.Logic.EnemySpawners;
using CodeBase.Infrastructure.Factories.EnemySpawner;
using CodeBase.Infrastructure.Factories.LevelTransfer;
using CodeBase.Infrastructure.Factories.Loot;
using CodeBase.Infrastructure.Factories.SaveTrigger;
using CodeBase.Services;
using CodeBase.Services.LevelCleared;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Respawn;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Level;
using CodeBase.UI.Services.Factory;
using UnityEngine;
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
    private readonly ILevelClearedService _levelClearedService;

    public InitLevelState(
      GameStateMachine gameStateMachine,
      IStaticDataService staticData,
      IPersistentProgressService progressService,
      IEnemySpawnerFactory enemySpawnerFactory,
      ISaveTriggerFactory saveTriggerFactory,
      ILevelTransferFactory levelTransferFactory,
      ILootFactory lootFactory,
      IUIFactory uiFactory,
      IRespawnService respawnService,
      ILevelClearedService levelClearedService
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
      _levelClearedService = levelClearedService;
    }

    public async void Enter()
    {
      LevelStaticData levelData = _staticData.ForLevel(CurrentLevelName());
      
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
      _levelClearedService.InitializeSpawners(spawners);
    }

    private async Task InitSaveTriggers(LevelStaticData levelStaticData)
    {
      foreach (SaveTriggerStaticData saveTriggerData in levelStaticData.SaveTriggers)
        await _saveTriggerFactory.
          CreateSaveTrigger(saveTriggerData.Id, saveTriggerData.TransformData, saveTriggerData.BoxColliderData, saveTriggerData.FirePosition);
    }

    private async Task InitLootPieces()
    {
      Dictionary<string,LootPieceDictionary> lootPiecesOnLevels = _progressService.Progress.WorldData.LootData.LootPiecesOnLevels.Dictionary;
      if(!lootPiecesOnLevels.TryGetValue(CurrentLevelName(), out LootPieceDictionary lootPieceDictionary))
        return;
      
      foreach (KeyValuePair<string, LootPieceData> item in lootPieceDictionary.Dictionary)
      {
        LootPiece lootPiece = await _lootFactory.CreateLoot();
        lootPiece.GetComponent<UniqueId>().Id = item.Key;
        lootPiece.Initialize(item.Value.Loot);
        lootPiece.transform.position = item.Value.Position.AsUnityVector();
      }
    }

    private async Task InitLevelTransfer(LevelStaticData levelData)
    {
      foreach (LevelTransferStaticData levelTransferData in levelData.LevelTransfers)
      {
        GameObject levelTransfer = await _levelTransferFactory.CreateLevelTransfer(levelTransferData.Position, levelTransferData.TransferTo);
        levelTransfer.SetActive(false);
        _levelClearedService.InitializeObjectToEnable(levelTransfer);
      }
    }
    private string CurrentLevelName() => 
      SceneManager.GetActiveScene().name;
  }
}