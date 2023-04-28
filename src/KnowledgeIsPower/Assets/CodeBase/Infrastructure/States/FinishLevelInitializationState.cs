using CodeBase.Gameplay.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;

namespace CodeBase.Infrastructure.States
{
  public class FinishLevelInitializationState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IPersistentProgressService _progressService;
    private readonly IProgressWatchers _progressWatchers;

    public FinishLevelInitializationState(
      GameStateMachine gameStateMachine,
      LoadingCurtain loadingCurtain,
      IPersistentProgressService progressService,
      IProgressWatchers progressWatchers
    )
    {
      _gameStateMachine = gameStateMachine;
      _loadingCurtain = loadingCurtain;
      _progressService = progressService;
      _progressWatchers = progressWatchers;
    }

    private void InformProgressReaders()
    {
      foreach (IProgressReader progressReader in _progressWatchers.Readers)
        progressReader.ReceiveProgress(_progressService.Progress);
    }

    public void Enter()
    {
      InformProgressReaders();
      _gameStateMachine.Enter<GameLoopState>();
    }

    public void Exit() =>
      _loadingCurtain.Hide();
  }
}