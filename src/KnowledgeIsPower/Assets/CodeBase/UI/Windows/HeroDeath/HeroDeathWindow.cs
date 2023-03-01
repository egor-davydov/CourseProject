using CodeBase.Services.Ads;
using CodeBase.Services.LifeCycle;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Windows.Ads;

namespace CodeBase.UI.Windows.HeroDeath
{
  public class HeroDeathWindow : WindowBase
  {
    public RewardedAds RewardedAds;
    public AdsHeroResurrection Reward;

    public void Construct(IPersistentProgressService progressService, IAdsService adsService, IResurrectionService resurrectionService)
    {
      base.Construct(progressService);
      RewardedAds.Construct(adsService);
      Reward.Construct(resurrectionService);
    }
    
    protected override void Initialize() => 
      RewardedAds.Initialize();

    protected override void SubscribeUpdates() => 
      RewardedAds.Subscribe();

    protected override void Cleanup()
    {
      base.Cleanup();
      RewardedAds.Cleanup();
    }
  }
}