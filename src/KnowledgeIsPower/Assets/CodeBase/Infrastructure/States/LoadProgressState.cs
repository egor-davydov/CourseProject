using System;
using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Monster;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;
    private readonly IStaticDataService _staticDataService;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress, IStaticDataService staticDataService)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
      _staticDataService = staticDataService;
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
      
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
    }

    private void LoadProgressOrInitNew()
    {
      _progressService.Progress = 
        _saveLoadProgress.LoadProgress() 
        ?? NewProgress();
    }

    private PlayerProgress NewProgress()
    {
      var progress =  new PlayerProgress(initialLevel: "Main");

      HeroStaticData heroData = _staticDataService.ForHero();
      progress.HeroState.MaxHP = heroData.MaxHp;
      progress.HeroStats.Damage = heroData.Damage;
      progress.HeroStats.DamageRadius = heroData.DamageRadius;
      progress.HeroStats.MaxDamageToCompleteBlock = heroData.MaxDamageToCompleteBlock;
      progress.HeroStats.DefendFactor = heroData.DefendFactor;
      progress.HeroStats.BasicMovementSpeed = heroData.BasicMovementSpeed;
      progress.HeroStats.FocusedMovementSpeed = heroData.FocusedMovementSpeed;
      progress.HeroStats.RotationSpeed = heroData.RotationSpeed;
      progress.HeroState.ResetHP();

      return progress;
    }
  }
}