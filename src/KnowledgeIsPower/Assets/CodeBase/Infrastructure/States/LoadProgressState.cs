using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;
    private readonly IStaticDataService _staticData;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress, IStaticDataService staticData)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
      _staticData = staticData;
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

      progress.HeroState.MaxHP = _staticData.HeroData.Hp;
      progress.HeroStats.Damage = _staticData.HeroData.Damage;
      progress.HeroStats.DamageRadius = _staticData.HeroData.DamageRadius;
      progress.HeroState.ResetHP();

      return progress;
    }
  }
}