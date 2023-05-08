using System.Threading.Tasks;
using CodeBase.Gameplay.Logic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Services.ProgressWatchers;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.LevelTransfer
{
  public class LevelTransferFactory : ILevelTransferFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IGameStateMachine _gameStateMachine;
    private readonly ISaveLoadService _saveLoadService;

    public LevelTransferFactory(IAssetProvider assets, IProgressWatchers progressWatchers, IGameStateMachine gameStateMachine, ISaveLoadService saveLoadService)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _gameStateMachine = gameStateMachine;
      _saveLoadService = saveLoadService;
    }

    public async Task<GameObject> CreateLevelTransfer(Vector3 at, string transferTo)
    {
      GameObject levelTransferObject = await _assets.Instantiate(AssetAddress.LevelTransferTrigger, at);
      _progressWatchers.Register(levelTransferObject);
      LevelTransferTrigger levelTransfer = levelTransferObject.GetComponent<LevelTransferTrigger>();

      levelTransfer.Construct(transferTo, _gameStateMachine, _saveLoadService);
      return levelTransferObject;
    }
  }
}