using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public interface IInputService : IService
  {
    Vector2 Axis { get; }

    bool IsFastAttackButtonUp();
    bool IsLongAttackButtonUp();
    bool IsDefendButtonUp();
  }
}