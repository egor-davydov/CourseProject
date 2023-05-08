using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Gameplay.Logic
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    private IGameStateMachine _stateMachine;
    private bool _triggered;
    private string _transferTo;

    public void Construct(IGameStateMachine stateMachine, string transferTo)
    {
      _transferTo = transferTo;
      _stateMachine = stateMachine;
    }

    private void OnTriggerEnter(Collider other)
    {
      if(_triggered)
        return;

      _stateMachine.Enter<LoadLevelState, string>(_transferTo);
      _triggered = true;
    }
  }
}