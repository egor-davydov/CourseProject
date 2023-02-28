using System;
using System.Collections.Generic;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.UI.Services.Windows
{
  public class WindowService : IWindowService
  {
    private readonly IUIFactory _uiFactory;
    private readonly IStaticDataService _staticData;

    private readonly Stack<WindowId> _previousWindows = new Stack<WindowId>();
    private WindowId _currentWindow = WindowId.Unknown;
    public event Action NewWindowOpened;

    public WindowService(IUIFactory uiFactory, IStaticDataService staticData)
    {
      _uiFactory = uiFactory;
      _staticData = staticData;
      _previousWindows.Push(_currentWindow);
    }

    public void OpenByButton(WindowId openingWindow)
    {
      if(openingWindow == WindowId.Unknown)
        return;

      if(AtLeastOneWindowOnScene())
      {
        CloseCurrentWindow();
        if(WindowSavingPreviousWindow(openingWindow))
          RememberCurrentWindow();
      }
      Open(openingWindow);
      DebugWindows();
    }

    private void Open(WindowId openingWindow)
    {
      _currentWindow = openingWindow;
      _uiFactory.CreateWindow(openingWindow);
    }

    private void RememberCurrentWindow() => 
      _previousWindows.Push(_currentWindow);

    private bool AtLeastOneWindowOnScene() => 
      _currentWindow != WindowId.Unknown;

    private void CloseCurrentWindow() => 
      NewWindowOpened?.Invoke();

    public void OpenPrevious()
    {
      if (_previousWindows.Peek() == WindowId.Unknown)
      {
        _currentWindow = WindowId.Unknown;
        return;
      }
      
      Open(_previousWindows.Pop());
    }

    private bool WindowSavingPreviousWindow(WindowId windowId) => 
      _staticData.ForWindow(windowId).OpenPreviousWindowOnClose;

    private void DebugWindows()
    {
      Debug.Log($"_currentWindow {_currentWindow}");
      int counter = 1;
      foreach (WindowId previousWindow in _previousWindows) 
        Debug.Log($"{counter++}. {previousWindow}");
    }
  }
}