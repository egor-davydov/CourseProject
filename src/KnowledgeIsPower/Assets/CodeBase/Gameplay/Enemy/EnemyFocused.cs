using UnityEngine;

namespace CodeBase.Gameplay.Enemy
{
  public class EnemyFocused : MonoBehaviour
  {
    [SerializeField]
    private GameObject _focusCircle;

    public void Focus() => 
      _focusCircle.SetActive(true);

    public void UnFocus() => 
      _focusCircle.SetActive(false);
  }
}