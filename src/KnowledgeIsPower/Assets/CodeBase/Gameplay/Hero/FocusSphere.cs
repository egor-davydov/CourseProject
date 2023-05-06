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
      if (!IsEnemyHurtBox(obj))
        return;
      
      Transform enemyTransform = obj.transform.parent;
      //Debug.Log($"OnSphereEnter {enemyTransform.name}");
      EnemiesInSphere.Add(enemyTransform);
      enemyTransform.GetComponent<EnemyDeath>().Happened += OnHappened;

      void OnHappened() =>
        RemoveInSphereAndTryChangeFocus(enemyTransform);
    }

    private void OnSphereExit(Collider obj)
    {
      if (!IsEnemyHurtBox(obj))
        return;
      
      Transform enemyTransform = obj.transform.parent;
      //Debug.Log($"OnSphereExit {enemyTransform.name}");
      if (EnemiesInSphere.Contains(enemyTransform)) 
        RemoveInSphereAndTryChangeFocus(enemyTransform);
    }

    private bool IsEnemyHurtBox(Collider obj) =>
      obj.CompareTag(Tags.EnemyHurtBox);

    private void RemoveInSphereAndTryChangeFocus(Transform objTransform)
    {
      EnemiesInSphere.Remove(objTransform);
      if (EnemiesInSphere.Count != 0)
        ChangeFocus();
    }

    private void ChangeFocus() => 
      GetComponentInParent<HeroFocusOnEnemy>().ChangeEnemyToFocusRight();
  }
}