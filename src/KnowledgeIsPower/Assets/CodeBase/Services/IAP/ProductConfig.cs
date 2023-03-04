using System;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
  [Serializable]
  public class ProductConfig
  {
    public string Id;
    public ProductType Type;

    public int MaxPurchaseCount;
  }
}