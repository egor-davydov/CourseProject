using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class FocusSphere : MonoBehaviour
  {
    private const string FocusTag = "Enemy";

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
      if (obj.transform.parent != null && obj.transform.parent.CompareTag(FocusTag))
      {
        Debug.Log("Add");
        EnemiesInSphere.Add(obj.transform.parent);
      }
      //Debug.Log(obj.name);
    }

    private void OnSphereExit(Collider obj)
    {
      if (obj.transform.parent != null && obj.transform.parent.CompareTag(FocusTag))
      {
        Debug.Log("Remove");
        EnemiesInSphere.Remove(obj.transform.parent);
      }
    }
  }
}