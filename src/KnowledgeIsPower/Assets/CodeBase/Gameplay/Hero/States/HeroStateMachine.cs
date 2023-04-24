using System.Collections.Generic;

namespace CodeBase.Gameplay.Hero.States
{
  public class HeroStateMachine : IHeroStateMachine
  {
    public HeroStateType CurrentStateType { get; private set; }
    public bool IsOnBasicState => CurrentStateType == HeroStateType.Basic; 
    public bool IsFocused => CurrentStateType == HeroStateType.Focused; 
    
    private readonly Dictionary<HeroStateType, IHeroState> _heroStates;

    public HeroStateMachine()
    {
      _heroStates = new Dictionary<HeroStateType, IHeroState>
      {
        [HeroStateType.Basic] = new HeroBasicState(),
        [HeroStateType.Focused] = new HeroFocusedState(),
      };
    }
    
    public void Enter(HeroStateType heroStateType)
    {
      IHeroState state = _heroStates[heroStateType];
      CurrentStateType = heroStateType;
      state.Enter();
    }
  }
}