using CodeBase.Services.LifeCycle;
using UnityEngine;

namespace CodeBase.Hero
{
  public class HeroResurrection : MonoBehaviour
  {
    public HeroDeath HeroDeath;

    private IResurrectionService _resurrectionService;

    public void Construct(IResurrectionService resurrectionService)
    {
      _resurrectionService = resurrectionService;
      _resurrectionService.OnResurrection += Resurrect;
    }
    
    private void OnDestroy() => 
      _resurrectionService.OnResurrection -= Resurrect;

    private void Resurrect() => 
      HeroDeath.MakeAlive();
  }
}