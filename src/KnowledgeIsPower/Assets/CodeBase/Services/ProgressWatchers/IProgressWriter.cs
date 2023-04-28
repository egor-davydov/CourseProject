using CodeBase.Data.Progress;

namespace CodeBase.Services.ProgressWatchers
{
  public interface IProgressWriter
  {
    void UpdateProgress(PlayerProgress progress);
  }
}