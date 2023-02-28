using System;
using CodeBase.Services;
using CodeBase.UI.Windows;

namespace CodeBase.UI.Services.Windows
{
  public interface IWindowService : IService
  {
    void OpenByButton(WindowId windowId);
    void OpenPrevious();
    event Action NewWindowOpened;
  }
}