using CodeBase.Services.Ads;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Windows.Ads;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
  public class ShopWindow : WindowBase
  {
    public TextMeshProUGUI SkullText;
    public RewardedAds RewardedAds;
    public AdsItem AdsItem;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
      base.Construct(progressService);
      RewardedAds.Construct(adsService);
      AdsItem.Construct(adsService, progressService);
    }
    
    protected override void Initialize()
    {
      RewardedAds.Initialize();
      RefreshSkullText();
    }

    protected override void SubscribeUpdates()
    {
      RewardedAds.Subscribe();
      Progress.WorldData.LootData.Changed += RefreshSkullText;
    }

    protected override void Cleanup()
    {
      base.Cleanup();
      RewardedAds.Cleanup();
      Progress.WorldData.LootData.Changed -= RefreshSkullText;
    }

    private void RefreshSkullText() => 
      SkullText.text = Progress.WorldData.LootData.Collected.ToString();
  }
}