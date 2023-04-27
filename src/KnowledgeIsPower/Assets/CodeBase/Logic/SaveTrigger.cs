using CodeBase.Gameplay;
using CodeBase.Services;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
  public class SaveTrigger : MonoBehaviour
  {
    private ISaveLoadService _saveLoadService;

    public BoxCollider Collider;

    private void Awake()
    {
      _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
    }

    private void OnTriggerEnter(Collider other)
    {
      if(!other.CompareTag(Tags.PlayerTag))
        return;
      
      _saveLoadService.SaveProgress();
      Debug.Log("Progress saved!");
      gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
      if(!Collider) return;
      
      Gizmos.color = new Color32(30, 200, 30, 130);
      Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
    }
  }
}