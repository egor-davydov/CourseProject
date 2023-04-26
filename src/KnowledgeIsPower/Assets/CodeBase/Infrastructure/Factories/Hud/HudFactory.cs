using System.Threading.Tasks;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.ProgressWatchers;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Hud
{
  public class HudFactory : IHudFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IHeroStateMachine _heroStateMachine;
    private readonly HeroProvider _heroProvider;
    private readonly IWindowService _windowService;

    public HudFactory(IAssetProvider assets, IProgressWatchers progressWatchers, IHeroStateMachine heroStateMachine, HeroProvider heroProvider, IWindowService windowService)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _heroStateMachine = heroStateMachine;
      _heroProvider = heroProvider;
      _windowService = windowService;
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await _assets.Instantiate(AssetAddress.HudPath);
      _progressWatchers.Register(hud);
      hud.GetComponentInChildren<FocusOnEnemyArea>()
        .Initialize(_heroStateMachine, _heroProvider.HeroObject);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
        openWindowButton.Init(_windowService);

      return hud;
    }
  }
}