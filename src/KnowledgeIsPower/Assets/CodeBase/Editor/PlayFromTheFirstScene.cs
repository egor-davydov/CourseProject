using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
  public static class PlayFromTheFirstScene
  {
    private const string PlayFromFirstSceneMenu = "Tools/Always Start From Scene 0 &p";

    private static bool UserWantToPlayFromFirstScene
    {
      get => EditorPrefs.HasKey(PlayFromFirstSceneMenu) && EditorPrefs.GetBool(PlayFromFirstSceneMenu);
      set => EditorPrefs.SetBool(PlayFromFirstSceneMenu, value);
    }

    [MenuItem(PlayFromFirstSceneMenu, isValidateFunction: false)]
    private static void PlayFromFirstSceneCheckMenu()
    {
      UserWantToPlayFromFirstScene = !UserWantToPlayFromFirstScene;
      Menu.SetChecked(PlayFromFirstSceneMenu, UserWantToPlayFromFirstScene);

      ShowNotifyOrLog(UserWantToPlayFromFirstScene ? "Play from scene 0" : "Play from current scene");
    }

    [MenuItem(PlayFromFirstSceneMenu, isValidateFunction: true)]
    private static bool PlayFromFirstSceneCheckMenuValidate()
    {
      Menu.SetChecked(PlayFromFirstSceneMenu, UserWantToPlayFromFirstScene);
      return true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadFirstSceneAtGameBegins()
    {
      if (!UserWantToPlayFromFirstScene || AlreadyOnFirstScene())
        return;
      
      if (EditorBuildSettings.scenes.Length == 0)
      {
        Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
        return;
      }

      SceneManager.LoadScene(0);
    }

    private static bool AlreadyOnFirstScene() => 
      SceneManager.GetActiveScene().name == "Initial";

    private static void ShowNotifyOrLog(string message)
    {
      if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
        EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(message));
      else
        Debug.Log(message); // When there's no scene view opened, we just print a log
    }
  }
}