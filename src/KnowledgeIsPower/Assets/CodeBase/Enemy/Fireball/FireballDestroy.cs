using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy.Fireball
{
  public class FireballDestroy : MonoBehaviour
  {
    public float FlyDuration = 4f;

    private bool _isDestroyed;

    private void Awake()
    {
      StartCoroutine(FlyTimer());

      _isDestroyed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
      _isDestroyed = true;
      Destroy(gameObject);

    }
    private IEnumerator FlyTimer()
    {
      yield return new WaitForSeconds(FlyDuration);
      
      if (!_isDestroyed)
        Destroy(gameObject);
    }

  }
}