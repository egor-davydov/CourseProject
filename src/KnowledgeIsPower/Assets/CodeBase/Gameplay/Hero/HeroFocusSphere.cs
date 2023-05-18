using System.Collections.Generic;
using CodeBase.Gameplay.Enemy;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroFocusSphere : MonoBehaviour
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
      if (!IsEnemyHurtBox(obj))
        return;

      Transform enemyTransform = obj.transform.parent;
      //Debug.Log($"OnSphereEnter {enemyTransform.name}");
      EnemiesInSphere.Add(enemyTransform);
      enemyTransform.GetComponent<EnemyDeath>().Happened += OnHappened;

      void OnHappened() =>
        RemoveFromSphereAndInform(enemyTransform);
    }

    private void OnSphereExit(Collider obj)
    {
      if (!IsEnemyHurtBox(obj))
        return;

      Transform enemyTransform = obj.transform.parent;
      //Debug.Log($"OnSphereExit {enemyTransform.name}");
      RemoveFromSphereAndInform(enemyTransform);
    }

    private bool IsEnemyHurtBox(Collider obj) =>
      obj.CompareTag(Tags.EnemyHurtBox);

    private void RemoveFromSphereAndInform(Transform enemyTransform)
    {
      if (EnemiesInSphere.Contains(enemyTransform))
        EnemiesInSphere.Remove(enemyTransform);
      InformAboutSphereLeaving();
    }

    private void InformAboutSphereLeaving() =>
      GetComponentInParent<HeroFocusOnEnemy>().EnemyLeftFromSphere();
  }
}