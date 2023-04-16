using System;
using System.Collections.Generic;

namespace CodeBase.Services.IAP
{
  public interface IIAPService : IService
  {
    event Action Initialized;
    bool IsInitialized { get; }
    void Initialize();
    List<ProductDescription> Products();
    void StartPurchase(string productId);
  }
}