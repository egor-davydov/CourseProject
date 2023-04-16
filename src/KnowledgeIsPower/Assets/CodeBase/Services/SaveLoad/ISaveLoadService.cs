using CodeBase.Data;
using CodeBase.Data.Progress;

namespace CodeBase.Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    PlayerProgress LoadProgress();
  }
}