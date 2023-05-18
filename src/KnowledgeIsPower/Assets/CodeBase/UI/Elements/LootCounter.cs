using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Data.Progress.Loot;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class LootCounter : MonoBehaviour, IProgressReader
  {
    public TextMeshProUGUI Counter;
    private LootData _lootData;

    public void ReceiveProgress(PlayerProgress progress)
    {
      _lootData = progress.WorldData.LootData;
      _lootData.Changed += UpdateCounter;
      
      UpdateCounter();
    }

    private void UpdateCounter() => 
      Counter.text = $"{_lootData.Collected}";
  }
}