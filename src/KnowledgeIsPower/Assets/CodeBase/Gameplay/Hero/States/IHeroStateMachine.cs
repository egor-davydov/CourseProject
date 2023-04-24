using CodeBase.Services;

namespace CodeBase.Gameplay.Hero.States
{
  public interface IHeroStateMachine : IService
  {
    HeroStateType CurrentStateType { get; }
    bool IsOnBasicState { get; }
    bool IsFocused { get; }
    void Enter(HeroStateType heroStateType);
  }
}