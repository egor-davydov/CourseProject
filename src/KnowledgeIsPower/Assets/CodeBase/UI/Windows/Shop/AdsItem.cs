using System.Collections;
using System.Collections.Generic;
using CodeBase.Services.Ads;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
  public class AdsItem : MonoBehaviour
  {
    public Button ShowAdButton;
    public GameObject[] AdActiveObjects;
    public GameObject[] AdInactiveObjects;
    
    private IPersistentProgressService _progressService;
    private IAdsService _adsService;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
      _adsService = adsService;
      _progressService = progressService;
    }

    public void Initialize()
    {
      ShowAdButton.onClick.AddListener(OnShowAdClicked);
      RefreshAvailableAd();
    }

    public void Subscribe() => 
      _adsService.RewardedVideoReady += RefreshAvailableAd;

    public void Cleanup() => 
      _adsService.RewardedVideoReady -= RefreshAvailableAd;

    private void OnShowAdClicked() => 
      _adsService.ShowRewardedVideo(OnVideoFinished);

    private void OnVideoFinished() => 
      _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);

    private void RefreshAvailableAd()
    {
      bool videoReady = _adsService.IsRewardedVideoReady;

      foreach (GameObject adActiveObject in AdActiveObjects) 
        adActiveObject.SetActive(videoReady);
      
      foreach (GameObject adInactiveObject in AdInactiveObjects) 
        adInactiveObject.SetActive(!videoReady);
    }
  }
}