using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Data.Progress.Loot;
using CodeBase.Extensions;
using CodeBase.Gameplay.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Gameplay.Enemy.Loot
{
  public class LootPiece : MonoBehaviour, IProgressWriter
  {
    public GameObject Skull;
    public GameObject PickupFxPrefab;
    public GameObject PickupPopup;
    public TextMeshPro LootText;

    private LootData _lootData;

    private LootPieceDictionary LootPieceDictionary
    {
      get
      {
        LootPiecesOnLevelsDictionary lootDataLootPiecesOnLevels = _lootData.LootPiecesOnLevels;
        if(!lootDataLootPiecesOnLevels.Dictionary.ContainsKey(CurrentLevelName()))
          lootDataLootPiecesOnLevels.Dictionary.Add(CurrentLevelName(), new LootPieceDictionary());
        
        return lootDataLootPiecesOnLevels.Dictionary[CurrentLevelName()];
      }
    }

    private Data.Progress.Loot.Loot _loot;

    private const float DelayBeforeDestroying = 1.5f;

    private string _id;
    
    private bool _pickedUp;

    public void Construct(LootData lootData) => 
      _lootData = lootData;

    public void Initialize(Data.Progress.Loot.Loot loot) => 
      _loot = loot;

    private void Start() => 
      _id = GetComponent<UniqueId>().Id;

    private void OnTriggerEnter(Collider other)
    {
      if (!_pickedUp)
      {
        _pickedUp = true;
        Pickup();
      }
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (_pickedUp)
        return;

      LootPieceDictionary lootPiecesOnScene = LootPieceDictionary;

      if (!lootPiecesOnScene.Dictionary.ContainsKey(_id))
        lootPiecesOnScene.Dictionary
          .Add(_id, new LootPieceData(transform.position.AsVectorData(), _loot));
    }

    private void Pickup()
    {
      UpdateWorldData();
      HideSkull();
      PlayPickupFx();
      ShowText();

      Destroy(gameObject, DelayBeforeDestroying);
    }

    private void UpdateWorldData()
    {
      UpdateCollectedLootAmount();
      RemoveLootPieceFromSavedPieces();
    }

    private void UpdateCollectedLootAmount() =>
      _lootData.Collect(_loot);

    private void RemoveLootPieceFromSavedPieces()
    {
      LootPieceDictionary savedLootPieces = LootPieceDictionary;

      if (savedLootPieces.Dictionary.ContainsKey(_id)) 
        savedLootPieces.Dictionary.Remove(_id);
    }

    private void HideSkull() =>
      Skull.SetActive(false);

    private void PlayPickupFx() =>
      Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

    private void ShowText()
    {
      LootText.text = $"{_loot.Value}";
      PickupPopup.SetActive(true);
    }
    
    private string CurrentLevelName() => 
      SceneManager.GetActiveScene().name;
  }
}