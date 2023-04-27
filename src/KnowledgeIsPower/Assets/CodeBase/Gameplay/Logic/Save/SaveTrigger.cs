using System.Collections.Generic;
using CodeBase.Data.Progress;
using CodeBase.Services.ProgressWatchers;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Gameplay.Logic.Save
{
  public class SaveTrigger : MonoBehaviour, IProgressReader, IProgressWriter
  {
    private ISaveLoadService _saveLoadService;
    private string _id;
    private bool _used;

    public void Construct(string id, ISaveLoadService saveLoadService)
    {
      _id = id;
      _saveLoadService = saveLoadService;
    }

    private void OnTriggerEnter(Collider other)
    {
      Debug.Log("Progress saved!");
      MakeTriggerUsed();
      _saveLoadService.SaveProgress();
    }

    public void ReceiveProgress(PlayerProgress progress)
    {
      if (!progress.WorldData.SaveTriggersData.UsedSaveTriggers.Contains(_id))
        return;

      MakeTriggerUsed();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      List<string> usedSaveTriggers = progress.WorldData.SaveTriggersData.UsedSaveTriggers;
      if (_used && !usedSaveTriggers.Contains(_id))
        usedSaveTriggers.Add(_id);
    }

    private void MakeTriggerUsed()
    {
      _used = true;
      gameObject.SetActive(false);
    }
  }
}