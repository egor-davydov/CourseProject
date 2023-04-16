using CodeBase.Data;
using CodeBase.Data.Progress;

namespace CodeBase.Services.PersistentProgress
{
  public interface ISavedProgress : ISavedProgressReader
  {
    void UpdateProgress(PlayerProgress progress);
  }
}