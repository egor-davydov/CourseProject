using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
  public class AdsService : IAdsService, IUnityAdsListener
  {
    private const string AndroidGameId = "4106937";
    private const string IOSGameId = "4106936";

    private const string UnityRewardedVideoIdAndroid = "Rewarded_Android";
    private const string UnityRewardedVideoIdIOS = "Rewarded_iOS";

    private string _gameId;
    private string _placementId;

    private Action _onVideoFinished;

    public event Action RewardedVideoReady;

    public int Reward => 15;

    public void Initialize()
    {
      SetIdsForCurrentPlatform();
      Advertisement.AddListener(this);
      Advertisement.Initialize(_gameId);
    }

    public void ShowRewardedVideo(Action onVideoFinished)
    {
      _onVideoFinished = onVideoFinished;
      Advertisement.Show(_placementId);
    }

    public bool IsRewardedVideoReady => Advertisement.IsReady(_placementId);

    public void OnUnityAdsReady(string placementId)
    {
#if DEBUGING
      Debug.Log($"OnUnityAdsReady {placementId}");
#endif

      if (placementId == _placementId)
        RewardedVideoReady?.Invoke();
    }

    public void OnUnityAdsDidError(string message)
    {
#if DEBUGING
      Debug.Log($"OnUnityAdsDidError {message}");
#endif
    }

    public void OnUnityAdsDidStart(string placementId)
    {
#if DEBUGING
      Debug.Log($"OnUnityAdsDidStart {placementId}");
#endif
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
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
      }

      _onVideoFinished = null;
    }

    private void SetIdsForCurrentPlatform()
    {
      switch (Application.platform)
      {
        case RuntimePlatform.Android:
          _gameId = AndroidGameId;
          _placementId = UnityRewardedVideoIdAndroid;
          break;

        case RuntimePlatform.IPhonePlayer:
          _gameId = IOSGameId;
          _placementId = UnityRewardedVideoIdIOS;
          break;

        case RuntimePlatform.WindowsEditor:
          _gameId = IOSGameId;
          _placementId = UnityRewardedVideoIdIOS;
          break;

        default:
          Debug.Log("Unsupported platform for ads.");
          break;
      }
    }
  }
}