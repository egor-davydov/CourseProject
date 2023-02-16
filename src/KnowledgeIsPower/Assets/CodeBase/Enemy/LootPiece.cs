using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Enemy
{
  public class LootPiece : MonoBehaviour, ISavedProgress
  {
    public GameObject Skull;
    public GameObject PickupFx;
    public GameObject PickupPopup;
    public TextMeshPro LootText;

    private Loot _loot;
    private bool _picked;
    private WorldData _worldData;
    private bool _destroyed;
    
    public void Construct(WorldData worldData) => 
      _worldData = worldData;

    public void Initialize(Loot loot) =>
      _loot = loot;

    private void OnTriggerEnter(Collider other) => PickUp();

    public void LoadProgress(PlayerProgress progress) =>
      _worldData = progress.WorldData;

    public void UpdateProgress(PlayerProgress progress)
    {
      if (_destroyed)
        return;

      string currentId = GetComponent<UniqueId>().Id;
      Debug.Log($" _picked: {_picked} InNotCollectedLoot: {InNotCollectedLoot(progress.WorldData, currentId)}");
      if (!_picked && !InNotCollectedLoot(progress.WorldData, currentId))
      {
        progress.WorldData.LootData.NotCollectedLoot.Add(new NotCollectedLoot(
          new PositionOnLevel(CurrentLevel(), CurrentPositionData()), _loot, currentId));
      }
    }

    private void PickUp()
    {
      if (_picked)
        return;

      _picked = true;

      if (InNotCollectedLoot(_worldData, GetComponent<UniqueId>().Id))
        RemoveFromNotCollectedLoot();

      UpdateWorldData();
      HideSkull();
      ShowText();
      PlayPickupFx();
      StartCoroutine(DestroyTimer());
    }

    private void RemoveFromNotCollectedLoot() =>
      _worldData.LootData.NotCollectedLoot.Remove(
        _worldData.LootData.NotCollectedLoot.First(loot => loot.Id == GetComponent<UniqueId>().Id));

    private bool InNotCollectedLoot(WorldData worldData, string currentId) =>
      NotCollectedIds(worldData).Contains(currentId);

    private IEnumerable<string> NotCollectedIds(WorldData worldData) =>
      worldData.LootData.NotCollectedLoot.Select(loot => loot.Id);

    private Vector3Data CurrentPositionData() =>
      transform.position.AsVectorData();

    private string CurrentLevel() =>
      SceneManager.GetActiveScene().name;

    private void UpdateWorldData() =>
      _worldData.LootData.Collect(_loot);

    private void PlayPickupFx() =>
      Instantiate(PickupFx, transform.position, Quaternion.identity);

    private void HideSkull() =>
      Skull.SetActive(false);

    private void ShowText()
    {
      LootText.text = $"{_loot.Value}";
      PickupPopup.SetActive(true);
    }

    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(1.5f);

      _destroyed = true;
      Destroy(gameObject);
    }
  }
}