using System.Threading.Tasks;
using CodeBase.Gameplay.Logic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;

    public LoadLevelState(
      GameStateMachine gameStateMachine,
      SceneLoader sceneLoader,
      LoadingCurtain loadingCurtain,
      IAssetProvider assets,
      IProgressWatchers progressWatchers
    )
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _assets = assets;
      _progressWatchers = progressWatchers;
    }

    public async void Enter(string sceneName)
    {
      _loadingCurtain.Show();

      Cleanup();
      await WarmUp();

      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
    }

    private void Cleanup()
    {
      _progressWatchers.Readers.Clear();
      _progressWatchers.Writers.Clear();

      _assets.Cleanup();
    }

    private async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.Loot);
      await _assets.Load<GameObject>(AssetAddress.Spawner);
    }

    private void OnLoaded() =>
      _stateMachine.Enter<InitLevelState>();
  }
}