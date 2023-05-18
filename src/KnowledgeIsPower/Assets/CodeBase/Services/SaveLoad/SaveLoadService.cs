using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Extensions;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
  public class SaveLoadService : ISaveLoadService
  {
    private const string ProgressKey = "Progress";

    private readonly IPersistentProgressService _progressService;
    private readonly IProgressWatchers _progressWatchers;

    public SaveLoadService(IPersistentProgressService progressService, IProgressWatchers progressWatchers)
    {
      _progressService = progressService;
      _progressWatchers = progressWatchers;
    }

    public void SaveProgress()
    {
      foreach (IProgressWriter progressWriter in _progressWatchers.Writers)
        progressWriter.UpdateProgress(_progressService.Progress);

      PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
    }

    public PlayerProgress LoadProgress()
    {
      return PlayerPrefs.GetString(ProgressKey)?
        .ToDeserialized<PlayerProgress>();
    }
  }
}