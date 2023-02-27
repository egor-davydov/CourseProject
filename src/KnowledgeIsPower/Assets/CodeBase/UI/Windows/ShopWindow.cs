using TMPro;

namespace CodeBase.UI.Windows
{
  public class ShopWindow : WindowBase
  {
    public TextMeshProUGUI SkullCount;

    protected override void Initialize() => 
      UpdateLootText();

    protected override void SubscribeUpdates() => 
      Progress.WorldData.LootData.Changed += UpdateLootText;

    private void UpdateLootText() => 
      SkullCount.text = Progress.WorldData.LootData.Collected.ToString();

    protected override void CleanUp()
    {
      base.CleanUp();
      Progress.WorldData.LootData.Changed -= UpdateLootText;
    }
  }
}