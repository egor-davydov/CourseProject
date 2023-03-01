using CodeBase.Services;
using CodeBase.UI.Windows;

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory: IService
  {
    void CreateShop();
    void CreateUIRoot();
    WindowBase CreateHeroDeathWindow();
  }
}