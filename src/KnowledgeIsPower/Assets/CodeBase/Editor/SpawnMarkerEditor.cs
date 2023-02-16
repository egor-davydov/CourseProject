using System;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(SpawnMarker))]
  public class SpawnMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo)
    {
      float k = (float)spawner.MonsterTypeId * 0.35f % 1f;
      Gizmos.color = new Color(k*0.9f, 0.5f, k*0.5f);
      Gizmos.DrawSphere(spawner.transform.position, 0.5f);
    }
  }
}