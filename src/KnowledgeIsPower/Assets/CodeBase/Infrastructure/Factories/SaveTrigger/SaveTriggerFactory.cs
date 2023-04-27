using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.ProgressWatchers;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.SaveTrigger
{
  public class SaveTriggerFactory : ISaveTriggerFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly ISaveLoadService _saveLoadService;

    public SaveTriggerFactory(IAssetProvider assets, IProgressWatchers progressWatchers, ISaveLoadService saveLoadService)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _saveLoadService = saveLoadService;
    }
    
    public async Task<GameObject> CreateSaveTrigger(string id, TransformData transformData, BoxColliderData boxColliderData)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.SaveTrigger);
      GameObject saveTriggerObject = Object.Instantiate(prefab, transformData.Position.AsUnityVector(), transformData.Rotation.AsUnityQuaternion());
      _progressWatchers.Register(saveTriggerObject);
      
      saveTriggerObject.GetComponent<Gameplay.Logic.Save.SaveTrigger>().Construct(id, _saveLoadService);
      BoxCollider boxCollider = saveTriggerObject.AddComponent<BoxCollider>();
      boxCollider.center = boxColliderData.Center;
      boxCollider.size = boxColliderData.Size;
      boxCollider.isTrigger = true;
      
      return saveTriggerObject;
    }
  }
}