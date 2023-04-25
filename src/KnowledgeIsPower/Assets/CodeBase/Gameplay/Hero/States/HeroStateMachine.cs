using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero.States
{
  public class HeroStateMachine : IHeroStateMachine, IUpdatable
  {
    private readonly Dictionary<HeroStateType, IHeroState> _heroStates;
    private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

    public HeroStateType CurrentStateType { get; private set; }
    public bool IsOnBasicState => CurrentStateType == HeroStateType.Basic;
    public bool IsFocused => CurrentStateType == HeroStateType.Focused;

    public HeroStateMachine()
    {
      _heroStates = new Dictionary<HeroStateType, IHeroState>
      {
        [HeroStateType.Basic] = new HeroBasicState(),
        [HeroStateType.Focused] = new HeroFocusedState(),
      };
      
      FillUpdatables();
    }

    public void Initialize(GameObject heroObject) => 
      ((HeroFocusedState)_heroStates[HeroStateType.Focused]).Initialize(heroObject);

    public void Enter(HeroStateType heroStateType)
    {
      IHeroState state = _heroStates[heroStateType];
      CurrentStateType = heroStateType;
      state.Enter();
    }

    public void OnUpdate()
    {
      foreach (IUpdatable updatable in _updatables) 
        updatable.OnUpdate();
    }

    private void FillUpdatables()
    {
      foreach (IHeroState heroState in _heroStates.Values)
        if (heroState is IUpdatable updatable)
          _updatables.Add(updatable);
    }
  }
}