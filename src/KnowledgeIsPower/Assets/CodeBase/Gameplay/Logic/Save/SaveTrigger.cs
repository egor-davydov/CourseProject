using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Gameplay.Logic.Save
{
  public class SaveTrigger : MonoBehaviour
  {
    private ISaveLoadService _saveLoadService;
    private string _id;

    public void Construct(string id, ISaveLoadService saveLoadService)
    {
      _id = id;
      _saveLoadService = saveLoadService;
    }

    private void OnTriggerEnter(Collider other)
    {
      if(!other.CompareTag(Tags.PlayerTag))
        return;
      
      _saveLoadService.SaveProgress();
      Debug.Log("Progress saved!");
      gameObject.SetActive(false);
    }
  }
}