using CodeBase.Data;
using CodeBase.Data.Progress;

namespace CodeBase.Services.PersistentProgress
{
  public interface IPersistentProgressService : IService
  {
    PlayerProgress Progress { get; set; }
  }
}