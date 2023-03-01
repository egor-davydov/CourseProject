using CodeBase.Services.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Ads
{
  public class RewardedAds : MonoBehaviour
  {
    public Button ShowAdButton;
    public GameObject[] AdActiveObjects;
    public GameObject[] AdInactiveObjects;

    private IAdsService _adsService;

    private IAdsReward _adsReward;

    public void Construct(IAdsService adsService)
    {
      _adsService = adsService;
      _adsReward = GetComponent<IAdsReward>();
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
      _adsReward.Give();
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