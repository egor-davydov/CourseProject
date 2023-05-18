using CodeBase.Services.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
  public static class AutoCollectLevelDataOnPlay
  {
    private const string AutoCollectOnPlayMenu = "Tools/Auto Collect On Play";
    
    private static IStaticDataService _staticDataService;

    private static IStaticDataService StaticDataService
    {
      get
      {
        if (_staticDataService == null)
        {
          _staticDataService = new StaticDataService();
          _staticDataService.Load();
        }

        return _staticDataService;
      }
    }

    private static bool AutoCollectOnPlay
    {
      get => EditorPrefs.HasKey(AutoCollectOnPlayMenu) && EditorPrefs.GetBool(AutoCollectOnPlayMenu);
      set => EditorPrefs.SetBool(AutoCollectOnPlayMenu, value);
    }
    
    [MenuItem(AutoCollectOnPlayMenu, isValidateFunction: false)]
    private static void PlayFromFirstSceneCheckMenu()
    {
      AutoCollectOnPlay = !AutoCollectOnPlay;
      Menu.SetChecked(AutoCollectOnPlayMenu, AutoCollectOnPlay);
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CollectLevelStaticData()
    {
      if (!AutoCollectOnPlay)
        return;
      
      for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
      {
        SceneManager.LoadScene(i);
        int index = SceneManager.GetActiveScene().buildIndex;
        LevelStaticDataEditor.Collect(StaticDataService.ForLevel(CurrentSceneName));
      }
    }

    private static string CurrentSceneName => SceneManager.GetActiveScene().name;
  }
}