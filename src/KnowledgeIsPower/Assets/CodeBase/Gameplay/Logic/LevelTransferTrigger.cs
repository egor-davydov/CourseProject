using CodeBase.Infrastructure.States;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Gameplay.Logic
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    private IGameStateMachine _stateMachine;
    private bool _triggered;
    private string _transferTo;
    private ISaveLoadService _saveLoadService;

    public void Construct(string transferTo, IGameStateMachine stateMachine, ISaveLoadService saveLoadService)
    {
      _saveLoadService = saveLoadService;
      _transferTo = transferTo;
      _stateMachine = stateMachine;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (_triggered)
        return;
      
      _saveLoadService.SaveProgress();
      _stateMachine.Enter<LoadLevelState, string>(_transferTo);
      _triggered = true;
    }
  }
}