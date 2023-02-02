using UnityEngine;

namespace CodeBase.Enemy.Fireball
{
  public class FireballMove : MonoBehaviour
  {
    public float Speed = 3f;

    private void Update() =>
      transform.Translate(Vector3.forward * Speed * Time.deltaTime);
  }
}