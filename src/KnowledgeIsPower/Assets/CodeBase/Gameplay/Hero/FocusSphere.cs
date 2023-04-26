using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Enemy;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class FocusSphere : MonoBehaviour
  {
    private const string EnemyTag = "Enemy";

    [SerializeField]
    private TriggerObserver _triggerObserver;

    public List<Transform> EnemiesInSphere { get; } = new List<Transform>();

    private void Start()
    {
      _triggerObserver.TriggerEnter += OnSphereEnter;
      _triggerObserver.TriggerExit += OnSphereExit;
    }

    private void OnDestroy()
    {
      _triggerObserver.TriggerEnter -= OnSphereEnter;
      _triggerObserver.TriggerExit -= OnSphereExit;
    }

    private void OnSphereEnter(Collider obj)
    {
      Transform enemyTransform = obj.transform.parent;
      if (enemyTransform != null && enemyTransform.CompareTag(EnemyTag))
      {
        Debug.Log("OnSphereEnter");
        EnemiesInSphere.Add(enemyTransform);
        enemyTransform.GetComponent<EnemyDeath>().Happened += OnHappened;
        
        void OnHappened()
        {
          RemoveFromEnemiesInSphere(enemyTransform);
          if(EnemiesInSphere.Count != 0)
            GetComponentInParent<HeroFocusOnEnemy>().ChangeEnemyToFocusRight();
        }
      }
    }

    private void RemoveFromEnemiesInSphere(Transform enemyTransform) => 
      EnemiesInSphere.Remove(enemyTransform);

    private void OnSphereExit(Collider obj)
    {
      Transform enemyTransform = obj.transform.parent;
      if (enemyTransform != null && enemyTransform.CompareTag(EnemyTag))
      {
        Debug.Log("OnSphereExit");
        RemoveFromEnemiesInSphere(enemyTransform);
      }
    }
  }
}