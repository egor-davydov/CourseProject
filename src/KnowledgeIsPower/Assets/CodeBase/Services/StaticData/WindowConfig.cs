using System;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;

namespace CodeBase.Services.StaticData
{
  [Serializable]
  public class WindowConfig
  {
    public WindowId WindowId;
    public WindowBase Prefab;
  }
}