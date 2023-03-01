using CodeBase.Services.LifeCycle;
using CodeBase.UI.Windows.Ads;
using UnityEngine;

namespace CodeBase.UI.Windows.HeroDeath
{
  public class AdsHeroResurrection : MonoBehaviour, IAdsReward
  {
    private IResurrectionService _resurrectionService;

    public void Construct(IResurrectionService resurrectionService)
    {
      _resurrectionService = resurrectionService;
    }

    public void Give()
    {
      _resurrectionService.Resurrect();
    }
  }

}