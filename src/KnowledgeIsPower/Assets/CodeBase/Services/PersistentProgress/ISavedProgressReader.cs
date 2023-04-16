using CodeBase.Data;
using CodeBase.Data.Progress;

namespace CodeBase.Services.PersistentProgress
{
  public interface ISavedProgressReader
  {
    void LoadProgress(PlayerProgress progress);
  }
}