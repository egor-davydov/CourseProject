using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.IAP;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
  public class ShopItemsContainer : MonoBehaviour
  {
    private const string ShopItemAddress = "ShopItem";
    
    public GameObject[] ShopUnavailableObjects;
    public Transform Parent;

    private IAPService _iapService;
    private IPersistentProgressService _progressService;
    private IAssetProvider _assets;
    private List<GameObject> _shopItems;


    public void Construct(IAPService iapService, IPersistentProgressService progressService, IAssetProvider assets)
    {
      _assets = assets;
      _progressService = progressService;
      _iapService = iapService;
    }

    public void Initialize() => 
      RefreshAvailableItems();

    public void Subscribe()
    {
      _iapService.Initialized += RefreshAvailableItems;
      _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
    }

    public void Cleanup()
    {
      _iapService.Initialized -= RefreshAvailableItems;
      _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
    }

    private async void RefreshAvailableItems()
    {
      UpdateShopUnavailableObjects();

      if (_iapService.IsInitialized)
        return;

      foreach (GameObject shopItem in _shopItems) 
        Destroy(shopItem);

      foreach (ProductDescription productDescription in _iapService.Products())
      {
        GameObject shopItemObject = await _assets.Instantiate(ShopItemAddress, Parent);
        ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();
        _shopItems.Add(shopItemObject);
      }
    }

    private void UpdateShopUnavailableObjects()
    {
      foreach (GameObject shopUnavailableObject in ShopUnavailableObjects)
        shopUnavailableObject.SetActive(!_iapService.IsInitialized);
    }
  }
}