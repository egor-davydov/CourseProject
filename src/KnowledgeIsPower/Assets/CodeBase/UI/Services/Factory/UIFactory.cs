using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
  public class UIFactory : IUIFactory
  {
    private const string UIRootPath = "UI/UIRoot";
    
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progress;
    private IWindowService _windowService;

    private Transform _uiRoot;

    public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progress)
    {
      _assets = assets;
      _staticData = staticData;
      _progress = progress;
    }

    public void Construct(IWindowService windowService) => 
      _windowService = windowService;

    public void CreateWindow(WindowId windowId)
    {
      WindowConfig config = _staticData.ForWindow(windowId);
      WindowBase window = Object.Instantiate(config.Prefab, _uiRoot);
      window.Construct(_progress, _windowService);
    }

    public void CreateUIRoot() => 
      _uiRoot = _assets.Instantiate(UIRootPath).transform;
  }
}