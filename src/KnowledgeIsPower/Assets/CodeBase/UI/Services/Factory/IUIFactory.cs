using CodeBase.Services;
using CodeBase.UI.Services.Windows;

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    void CreateWindow(WindowId windowId);
    void CreateUIRoot();
    void Construct(IWindowService windowService);
  }
}