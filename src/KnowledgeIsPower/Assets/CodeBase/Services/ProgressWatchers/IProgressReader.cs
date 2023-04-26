using CodeBase.Data.Progress;

namespace CodeBase.Services.ProgressWatchers
{
  public interface IProgressReader
  {
    void ReceiveProgress(PlayerProgress progress);
  }
}