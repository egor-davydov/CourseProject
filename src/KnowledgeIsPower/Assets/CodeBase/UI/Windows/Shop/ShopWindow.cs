using CodeBase.Services.Ads;
using CodeBase.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
  public class ShopWindow : WindowBase
  {
    public TextMeshProUGUI SkullText;
    public AdsItem AdsItem;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
      base.Construct(progressService);
      AdsItem.Construct(adsService, progressService);
    }
    
    protected override void Initialize()
    {
      AdsItem.Initialize();
      RefreshSkullText();
    }

    protected override void SubscribeUpdates()
    {
      AdsItem.Subscribe();
      Progress.WorldData.LootData.Changed += RefreshSkullText;
    }

    protected override void Cleanup()
    {
      base.Cleanup();
      AdsItem.Cleanup();
      Progress.WorldData.LootData.Changed -= RefreshSkullText;
    }

    private void RefreshSkullText() => 
      SkullText.text = Progress.WorldData.LootData.Collected.ToString();
  }
}