using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Gameplay.Logic
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    public string TransferTo;
    private IGameStateMachine _stateMachine;
    private bool _triggered;

    public void Construct(IGameStateMachine stateMachine) => 
      _stateMachine = stateMachine;

    private void OnTriggerEnter(Collider other)
    {
      if(_triggered)
        return;

      if (!other.CompareTag(Tags.PlayerTag))
        return;
      
      _stateMachine.Enter<LoadLevelState, string>(TransferTo);
      _triggered = true;
    }
  }
}