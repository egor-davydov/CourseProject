using UnityEngine;

namespace CodeBase.Services.Input
{
  public abstract class InputService : IInputService
  {
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const string FastAttackName = "Fast";
    private const string LongAttackName = "Long";
    private const string DefendName = "Defend";

    public abstract Vector2 Axis { get; }


    public bool IsFastAttackButtonUp() => 
      SimpleInput.GetButtonUp(FastAttackName);

    public bool IsLongAttackButtonUp() => 
      SimpleInput.GetButtonUp(LongAttackName);

    public bool IsDefendButtonUp() => 
      SimpleInput.GetButtonUp(DefendName);

    protected static Vector2 SimpleInputAxis() => 
      new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
  }
}