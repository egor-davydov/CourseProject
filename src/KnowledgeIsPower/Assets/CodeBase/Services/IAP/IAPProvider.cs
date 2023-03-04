using System;
using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
  public class IAPProvider : IStoreListener
  {
    private const string IAPConfigsPath = "IAP/Products";

    private IStoreController _controller;
    private IExtensionProvider _extensions;

    private List<ProductConfig> _configs;

    public event Action Initialized;
    public bool IsInitialized => _controller != null && _extensions != null;


    public void Initialize()
    {
      ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
      
      Load();

      foreach (ProductConfig productConfig in _configs) 
        builder.AddProduct(productConfig.Id, productConfig.Type);

      UnityPurchasing.Initialize(this, builder);
    }

    public void StartPurchase(string productId) => 
      _controller.InitiatePurchase(productId);

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
      _extensions = extensions;
      _controller = controller;
      
      Initialized?.Invoke();
      Debug.Log("UnityPurchasing initialize success");
    }

    public void OnInitializeFailed(InitializationFailureReason error) => 
      Debug.Log($"UnityPurchasing OnInitializeFailed: {error}");

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
      Debug.Log($"UnityPurchasing ProcessPurchase success {purchaseEvent.purchasedProduct.definition.id}");
      
      return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) => 
      Debug.Log($"UnityPurchasing OnPurchaseFailed Product: {product.definition.id}, PurchaseFailureReason: {failureReason}, transaction id: {product.transactionID}");

    private void Load() => 
      _configs = Resources.Load<TextAsset>(IAPConfigsPath).text.ToDeserialized<ProductConfigWrapper>().Configs;
  }
}