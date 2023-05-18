using UnityEngine;

namespace CodeBase.Gameplay.Logic
{
  public class LevelTransferInitialPoint : MonoBehaviour
  {
    [SerializeField] private string _transferTo;
  
    public string TransferTo => _transferTo;
  }
}