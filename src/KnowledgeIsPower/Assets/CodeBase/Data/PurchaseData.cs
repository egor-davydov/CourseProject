using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace CodeBase.Data
{
  [Serializable]
  public class PurchaseData
  {
    private List<BoughtIAP> _boughtIAPs = new List<BoughtIAP>();

    public event Action Changed; 

    public void AddPurchase(string id)
    {
      BoughtIAP boughtIAP = Product(id);

      if (boughtIAP == null)
        _boughtIAPs.Add(new BoughtIAP { Id = id, Count = 1});
      else
        boughtIAP.Count++;
        
      Changed?.Invoke();
    }

    private BoughtIAP Product(string productId) => 
      _boughtIAPs.Find(x => x.Id == productId);
  }
}