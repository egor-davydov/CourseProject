using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
  public class AutoCollectLevelDataOnPlay
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
    
    public static bool AutoCollectOnPlay
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
      if (AutoCollectOnPlay)
      {
        for (int i = 1; i < EditorBuildSettings.scenes.Length; i++)
        {
          SceneManager.LoadScene(i);
          LevelStaticDataEditor.Collect(StaticDataService.ForLevel(CurrentSceneName));
        }
      }
    }

    private static string CurrentSceneName => SceneManager.GetActiveScene().name;
  }
}