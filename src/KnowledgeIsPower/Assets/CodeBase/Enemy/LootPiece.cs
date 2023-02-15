using System.Collections;
using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class LootPiece : MonoBehaviour
  {
    public GameObject Skull;
    public GameObject PickupFx;
    public GameObject PickupPopup;
    public TextMeshPro LootText;
    
    private Loot _loot;
    private bool _picked;
    private WorldData _worldData;

    public void Construct(WorldData worldData) => 
      _worldData = worldData;

    public void Initialize(Loot loot) => 
      _loot = loot;

    private void OnTriggerEnter(Collider other) => PickUp();

    private void PickUp()
    {
      if (_picked)
        return;
      
      _picked = true;

      UpdateWorldData();
      HideSkull();
      ShowText();
      PlayPickupFx();
      StartCoroutine(DestroyTimer());
    }

    private void UpdateWorldData() => 
      _worldData.LootData.Collect(_loot);

    private void PlayPickupFx() => 
      Instantiate(PickupFx, transform.position, Quaternion.identity);

    private void HideSkull() => 
      Skull.SetActive(false);

    private void ShowText()
    {
      LootText.text = $"{_loot.Value}";
      PickupPopup.SetActive(true);
    }

    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(1.5f);
      
      Destroy(gameObject);
    }
  }
}