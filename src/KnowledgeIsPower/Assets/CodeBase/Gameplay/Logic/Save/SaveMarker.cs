using UnityEngine;

namespace CodeBase.Gameplay.Logic.Save
{
  public class SaveMarker : MonoBehaviour
  {
    [SerializeField]
    private BoxCollider _boxCollider;

    public BoxCollider BoxCollider => _boxCollider;

    private void OnDrawGizmos()
    {
      if(!BoxCollider)
        return;
      
      Gizmos.color = new Color32(30, 200, 30, 130);
      Gizmos.DrawCube(transform.position + BoxCollider.center, BoxCollider.size);
    }
  }
}