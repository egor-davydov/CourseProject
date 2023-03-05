using System;
using CodeBase.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
  public class IAPService
  {
    private readonly IAPProvider _iapProvider;
    private readonly IPersistentProgressService _progress;

    public event Action Initialized;
    public bool IsInitialized => _iapProvider.IsInitialized;

    public IAPService(IAPProvider iapProvider, IPersistentProgressService progress)
    {
      _iapProvider = iapProvider;
      _progress = progress;
    }

    public void Initialize()
    {
      _iapProvider.Initialize(this);
      _iapProvider.Initialized += () => Initialized?.Invoke();
    }
    
    public void StartPurchase(string productId) => 
      _iapProvider.StartPurchase(productId);

    public PurchaseProcessingResult ProcessPurchase(Product product)
    {
      ProductConfig productConfig = _iapProvider.Configs[product.definition.id];
      
      switch (productConfig.ItemType)
      {
        case ItemType.Skulls: 
          _progress.Progress.WorldData.LootData.Add(productConfig.Quantity);
          _progress.Progress.PurchaseData.AddPurchase(product.definition.id);
          break;
      }

      return PurchaseProcessingResult.Complete;
    }
  }
}