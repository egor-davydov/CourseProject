using System;
using CodeBase.Services.LifeCycle;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Windows
{
  public class WindowService : IWindowService
  {
    private readonly IUIFactory _uiFactory;
    private readonly IResurrectionService _resurrectionService;

    public WindowService(IUIFactory uiFactory, IResurrectionService resurrectionService)
    {
      _uiFactory = uiFactory;
      _resurrectionService = resurrectionService;
    }

    public void Open(WindowId windowId)
    {
      switch (windowId)
      {
        case WindowId.None:
          break;
        case WindowId.Shop:
          _uiFactory.CreateShop();
          break;
        case WindowId.HeroDeath:
          WindowBase heroDeathWindow = _uiFactory.CreateHeroDeathWindow();
          _resurrectionService.OnResurrection += () => Close(heroDeathWindow);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
      }
    }

    private void Close(WindowBase window) => 
      UnityEngine.Object.Destroy(window.gameObject);
  }
}