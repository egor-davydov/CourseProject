using System;

namespace CodeBase.Services.LifeCycle
{
  public class ResurrectionService : IResurrectionService
  {
    public event Action OnResurrection;

    public void Resurrect() => 
      OnResurrection?.Invoke();
  }
}