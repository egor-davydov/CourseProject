using System;
using UnityEngine;

namespace CodeBase.Enemy.Fireball
{
  public class FireballHit : MonoBehaviour
  {
    public event Action<Collider> Happened;

    private void OnTriggerEnter(Collider other)
    {
      if (IsPlayer(other))
        Happened?.Invoke(other);
    }

    private bool IsPlayer(Collider other) =>
      other.CompareTag("Player");
  }
}