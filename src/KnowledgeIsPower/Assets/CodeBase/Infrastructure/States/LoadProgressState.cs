using System;
using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
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

      progress.HeroState.MaxHP = 100;
      progress.HeroStats.Damage = 20;
      progress.HeroStats.DamageRadius = 0.5f;
      progress.HeroStats.MaximumDamageToBlock = 10f;
      progress.HeroStats.DefendFactor = 0.5f;
      progress.HeroStats.BasicMovementSpeed = 5f;
      progress.HeroStats.FocusedMovementSpeed = 4f;
      progress.HeroState.ResetHP();

      return progress;
    }
  }
}