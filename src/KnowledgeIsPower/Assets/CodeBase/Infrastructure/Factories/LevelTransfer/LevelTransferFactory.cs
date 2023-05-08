using System.Threading.Tasks;
using CodeBase.Gameplay.Logic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.LevelTransfer
{
  public class LevelTransferFactory : ILevelTransferFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IGameStateMachine _gameStateMachine;

    public LevelTransferFactory(IAssetProvider assets, IProgressWatchers progressWatchers, IGameStateMachine gameStateMachine)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _gameStateMachine = gameStateMachine;
    }

    public async Task<GameObject> CreateLevelTransfer(Vector3 at)
    {
      GameObject levelTransferObject = await _assets.Instantiate(AssetAddress.LevelTransferTrigger, at);
      _progressWatchers.Register(levelTransferObject);
      LevelTransferTrigger levelTransfer = levelTransferObject.GetComponent<LevelTransferTrigger>();

      levelTransfer.Construct(_gameStateMachine);
      return levelTransferObject;
    }
  }
}