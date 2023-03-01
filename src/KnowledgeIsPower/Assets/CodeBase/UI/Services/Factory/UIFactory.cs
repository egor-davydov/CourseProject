using System.Runtime.Serialization;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.Ads;
using CodeBase.Services.LifeCycle;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.HeroDeath;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
  public class UIFactory : IUIFactory
  {
    private const string UIRootPath = "UI/UIRoot";
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly IAdsService _adsService;
    private readonly IResurrectionService _resurrectionService;
    
    private Transform _uiRoot;

    public UIFactory(
      IAssetProvider assets,
      IStaticDataService staticData,
      IPersistentProgressService progressService,
      IAdsService adsService,
      IResurrectionService resurrectionService
      )
    {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
      _adsService = adsService;
      _resurrectionService = resurrectionService;
    }

    public void CreateShop()
    {
      WindowConfig config = _staticData.ForWindow(WindowId.Shop);
      ShopWindow window = Object.Instantiate(config.Template, _uiRoot) as ShopWindow;
      window.Construct(_adsService, _progressService);
    }

    public WindowBase CreateHeroDeathWindow()
    {
      WindowConfig config = _staticData.ForWindow(WindowId.HeroDeath);
      HeroDeathWindow window = Object.Instantiate(config.Template, _uiRoot) as HeroDeathWindow;
      window.Construct(_progressService, _adsService, _resurrectionService);
      return window;
    }

    public void CreateUIRoot() => 
      _uiRoot = _assets.Instantiate(UIRootPath).transform;
  }
}