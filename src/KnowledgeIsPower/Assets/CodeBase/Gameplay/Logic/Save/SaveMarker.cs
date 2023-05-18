using UnityEngine;

namespace CodeBase.Gameplay.Logic.Save
{
  public class SaveMarker : MonoBehaviour
  {
    [SerializeField]
    private BoxCollider _boxCollider;

    [SerializeField]
    private GameObject _firePositionObject;

    public BoxCollider BoxCollider => _boxCollider;
    public GameObject FirePositionObject => _firePositionObject;

    private void OnDrawGizmos()
    {
      if(!_boxCollider)
        return;
      
      Gizmos.color = new Color32(30, 200, 30, 130);
      Gizmos.DrawCube(transform.position + _boxCollider.center, _boxCollider.size);
    }
  }
}