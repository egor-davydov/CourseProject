using System.Threading.Tasks;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.Input;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Hero
{
  public class HeroFactory : IHeroFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IHeroStateMachine _heroStateMachine;
    private readonly IInputService _inputService;

    public HeroFactory(IAssetProvider assets, IProgressWatchers progressWatchers, IHeroStateMachine heroStateMachine, IInputService inputService)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _heroStateMachine = heroStateMachine;
      _inputService = inputService;
    }
    
    public async Task<GameObject> CreateHero(Vector3 at)
    {
      GameObject heroObject = await _assets.Instantiate(AssetAddress.HeroPath, at);
      _progressWatchers.Register(heroObject);
      
      heroObject.GetComponent<HeroAnimator>().Construct(_heroStateMachine);
      heroObject.GetComponent<HeroMove>().Construct(_inputService, _heroStateMachine);
      heroObject.GetComponent<HeroDefend>().Construct(_inputService);
      
      return heroObject;
    }
  }
}