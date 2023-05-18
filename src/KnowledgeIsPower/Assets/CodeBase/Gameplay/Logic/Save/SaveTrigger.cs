using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data.Progress;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Infrastructure.AssetManagement;
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
    private Vector3 _firePosition;
    private IAssetProvider _assets;

    public void Construct(string id, Vector3 firePosition, ISaveLoadService saveLoadService, IAssetProvider assets)
    {
      _assets = assets;
      _id = id;
      _firePosition = firePosition;
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

    private async void MakeTriggerUsed()
    {
      _used = true;
      gameObject.SetActive(false);
      await CreateFire();
    }

    private async Task CreateFire()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Fire);
      Instantiate(prefab, _firePosition, prefab.transform.rotation);
    }
  }
}