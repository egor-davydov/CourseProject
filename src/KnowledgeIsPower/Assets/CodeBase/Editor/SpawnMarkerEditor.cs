using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(SpawnMarker))]
  public class SpawnMarkerEditor : UnityEditor.Editor
  {
    private static IStaticDataService _staticDataService;

    public static IStaticDataService StaticDataService
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
      Handles.Label(spawner.transform.position + Vector3.up * 1f, $"Damage: {data.Damage}");
      Handles.Label(spawner.transform.position + Vector3.up * 2f, $"Speed: {data.MoveSpeed}");
      Handles.Label(spawner.transform.position + Vector3.up * 3f, $"HP: {data.Hp}/{data.Hp}");
      SkinnedMeshRenderer mesh = data.PrefabReference.editorAsset.GetComponentInChildren<SkinnedMeshRenderer>();
      Gizmos.color = Color.red;
      Gizmos.DrawMesh(mesh.sharedMesh, spawner.transform.position, spawner.transform.rotation, mesh.transform.lossyScale);
    }
  }
}