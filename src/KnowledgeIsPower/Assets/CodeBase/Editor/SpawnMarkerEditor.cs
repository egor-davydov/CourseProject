using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(SpawnMarker))]
  public class SpawnMarkerEditor : UnityEditor.Editor
  {
    private static IStaticDataService _staticDataService;

    private static IStaticDataService StaticDataService
    {
      get
      {
        if (_staticDataService == null)
        {
          _staticDataService = new StaticDataService();
          _staticDataService.Load();
        }

        return _staticDataService;
      }
    }

    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo)
    {
      MonsterStaticData data = StaticDataService.ForMonster(spawner.MonsterTypeId);

      Transform spawnerTransform = spawner.transform;
      Vector3 spawnerPosition = spawnerTransform.position;

      Handles.Label(spawnerPosition + Vector3.up * 1f, $"Damage: {data.Damage}");
      Handles.Label(spawnerPosition + Vector3.up * 2f, $"Speed: {data.MoveSpeed}");
      Handles.Label(spawnerPosition + Vector3.up * 3f, $"HP: {data.Hp}/{data.Hp}");

      SkinnedMeshRenderer mesh = data.PrefabReference.editorAsset.GetComponentInChildren<SkinnedMeshRenderer>();
      Gizmos.color = Color.red;
      Gizmos.DrawMesh(mesh.sharedMesh, spawnerPosition, spawnerTransform.rotation, mesh.transform.lossyScale);
    }
  }
}