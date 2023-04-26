using System.Collections.Generic;
using CodeBase.Gameplay.Enemy;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class FocusSphere : MonoBehaviour
  {
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
      Transform transformParent = obj.transform.parent;
      if (IsEnemy(transformParent))
        return;

      EnemiesInSphere.Add(transformParent);
      transformParent.GetComponent<EnemyDeath>().Happened += OnHappened;

      void OnHappened()
      {
        EnemiesInSphere.Remove(transformParent);
        if (EnemiesInSphere.Count != 0)
          GetComponentInParent<HeroFocusOnEnemy>().ChangeEnemyToFocusRight();
      }
    }

    private void OnSphereExit(Collider obj)
    {
      Transform transformParent = obj.transform.parent;
      if (IsEnemy(transformParent))
        EnemiesInSphere.Remove(transformParent);
    }

    private bool IsEnemy(Transform transformParent) =>
      transformParent == null || !transformParent.CompareTag(Tags.EnemyTag);
  }
}