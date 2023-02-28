using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
  public abstract class WindowBase : MonoBehaviour
  {
    public Button CloseButton;

    protected IPersistentProgressService ProgressService;
    protected PlayerProgress Progress => ProgressService.Progress;
    
    private IWindowService _windowService;

    public void Construct(IPersistentProgressService progressService, IWindowService windowService)
    {
      _windowService = windowService;
      ProgressService = progressService;
      _windowService.NewWindowOpened += CloseWindow;
    }

    private void Awake() =>
      OnAwake();

    private void Start()
    {
      Initialize();
      SubscribeUpdates();
    }

    private void OnDestroy() =>
      CleanUp();

    protected virtual void Initialize()
    {
    }

    protected virtual void SubscribeUpdates()
    {
    }

    protected virtual void CleanUp() => 
      _windowService.NewWindowOpened -= CloseWindow;

    protected virtual void OnAwake()
    {
      CloseButton.onClick.AddListener(() =>
      {
        _windowService.OpenPrevious();
        CloseWindow();
      });
    }

    private void CloseWindow() => 
      Destroy(gameObject);
  }
}