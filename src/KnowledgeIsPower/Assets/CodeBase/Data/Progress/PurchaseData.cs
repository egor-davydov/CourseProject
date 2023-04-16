using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data.Progress
{
  [Serializable]
  public class PurchaseData
  {
    public List<BoughtIAP> BoughtIAPs = new List<BoughtIAP>();

    public event Action Changed; 

    public void AddPurchase(string id)
    {
      BoughtIAP boughtIAP = Product(id);

      Debug.Log(boughtIAP);
      if (boughtIAP == null)
        BoughtIAPs.Add(new BoughtIAP { Id = id, Count = 1});
      else
        boughtIAP.Count++;
        
      Changed?.Invoke();
    }

    public BoughtIAP Product(string productId) => 
      BoughtIAPs.Find(x => x.Id == productId);
  }
}