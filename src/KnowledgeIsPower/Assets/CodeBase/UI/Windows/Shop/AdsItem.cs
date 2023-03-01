using CodeBase.Services.Ads;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Windows.Ads;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
  public class AdsItem : MonoBehaviour, IAdsReward
  {
    private IPersistentProgressService _progressService;
    private IAdsService _adsService;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
      _adsService = adsService;
      _progressService = progressService;
    }

    public void Give() => 
      _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);
  }
}