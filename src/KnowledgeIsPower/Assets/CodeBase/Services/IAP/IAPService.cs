using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
  public class IAPService : IIAPService
  {
    private readonly IAPProvider _iapProvider;
    private readonly IPersistentProgressService _progress;
    private readonly IRespawnService _respawnService;

    public event Action Initialized;
    public bool IsInitialized => _iapProvider.IsInitialized;

    public IAPService(IAPProvider iapProvider, IPersistentProgressService progress, IRespawnService respawnService)
    {
      _respawnService = respawnService;
      _iapProvider = iapProvider;
      _progress = progress;
    }

    public void Initialize()
    {
      _iapProvider.Initialize(this);
      _iapProvider.Initialized += () => Initialized?.Invoke();
    }

    public List<ProductDescription> Products() =>
      ProductDescriptions().ToList();

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
        case ItemType.MonsterRespawn:
          _progress.Progress.KillData.ClearedSpawners.Clear();
          _progress.Progress.PurchaseData.AddPurchase(product.definition.id);
          _respawnService.RespawnEnemies();
          break;
      }

      return PurchaseProcessingResult.Complete;
    }

    private IEnumerable<ProductDescription> ProductDescriptions()
    {
      PurchaseData purchaseData = _progress.Progress.PurchaseData;
      foreach (string productId in _iapProvider.Products.Keys)
      {
        ProductConfig config = _iapProvider.Configs[productId];
        Product product = _iapProvider.Products[productId];

        BoughtIAP boughtIAP = purchaseData.Product(productId);

        if (ProductBoughtOut(boughtIAP, config))
          continue;

        yield return new ProductDescription
        {
          Id = productId,
          Product = product,
          Config = config,
          AvailablePurchasesLeft = boughtIAP != null
            ? config.MaxPurchaseCount - boughtIAP.Count
            : config.MaxPurchaseCount
        };
      }
    }

    private static bool ProductBoughtOut(BoughtIAP boughtIAP, ProductConfig config) => 
      boughtIAP != null && boughtIAP.Count >= config.MaxPurchaseCount;
  }
}