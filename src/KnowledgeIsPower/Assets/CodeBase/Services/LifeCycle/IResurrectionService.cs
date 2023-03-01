using System;

namespace CodeBase.Services.LifeCycle
{
  public interface IResurrectionService : IService
  {
    void Resurrect();
    event Action OnResurrection;
  }
}