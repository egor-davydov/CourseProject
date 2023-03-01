using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
  public class AdsService : IAdsService, IUnityAdsListener
  {
    public event Action RewardedVideoReady;
    
    private const string IOSGameId = "5185404";
    private const string AndroidGameId = "5185405";
    private const string RewardedVideoPlacementId = "Rewarded_Android";
    
    private Action _onVideoFinished;
    private string _gameId;

    public int Reward => 13;

    public void Initialize()
    {
      switch (Application.platform)
      {
        case RuntimePlatform.WindowsEditor:
          _gameId = AndroidGameId;
          break;
        case RuntimePlatform.IPhonePlayer:
          _gameId = IOSGameId;
          break;
        case RuntimePlatform.Android:
          _gameId = AndroidGameId;
          break;
        default:
          Debug.Log("Not supported platform");
          break;
      }

      Advertisement.AddListener(this);
      Advertisement.Initialize(_gameId);
    }

    public void ShowRewardedVideo(Action onVideoFinished)
    {
      Advertisement.Show(RewardedVideoPlacementId);
      
      _onVideoFinished = onVideoFinished;
    }

    public bool IsRewardedVideoReady =>
      Advertisement.IsReady(RewardedVideoPlacementId);
    
    public void OnUnityAdsReady(string placementId)
    {
      Debug.Log($"OnUnityAdsReady {placementId}");

      if (placementId == RewardedVideoPlacementId) 
        RewardedVideoReady?.Invoke();
    }

    public void OnUnityAdsDidError(string message) => 
      Debug.Log($"OnUnityAdsDidError {message}");

    public void OnUnityAdsDidStart(string placementId)
    {
      Debug.Log($"OnUnityAdsDidStart {placementId}");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
      switch (showResult)
      {
        case ShowResult.Failed:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
        case ShowResult.Skipped:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
        case ShowResult.Finished:
          _onVideoFinished?.Invoke();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(showResult), showResult, null);
      }
    }
  }
}