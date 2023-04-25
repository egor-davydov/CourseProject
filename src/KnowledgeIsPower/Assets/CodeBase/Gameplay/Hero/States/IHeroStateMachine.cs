using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Gameplay.Hero.States
{
  public interface IHeroStateMachine : IService
  {
    HeroStateType CurrentStateType { get; }
    bool IsOnBasicState { get; }
    bool IsFocused { get; }
    void Enter(HeroStateType heroStateType);
    void Initialize(GameObject heroObject);
  }
}