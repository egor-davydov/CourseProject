using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
  public class IAPProvider : IStoreListener
  {
    private const string IAPConfigsPath = "IAP/Products";
    
    private List<ProductConfig> _configs;

    public void Initialize()
    {
      ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
      
      Load();

      foreach (ProductConfig productConfig in _configs) 
        builder.AddProduct(productConfig.Id, productConfig.Type);

      UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
      throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
      throw new System.NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
      throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
      throw new System.NotImplementedException();
    }

    private void Load() => 
      _configs = Resources.Load<TextAsset>(IAPConfigsPath).text.ToDeserialized<ProductConfigWrapper>().Configs;
  }
}