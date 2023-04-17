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
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo)
    {
      IStaticDataService staticData = new StaticDataService();
      staticData.Load();

      MonsterStaticData data = staticData.ForMonster(spawner.MonsterTypeId);
      Handles.Label(spawner.transform.position + Vector3.up*1f, $"Damage: {data.Damage}");
      Handles.Label(spawner.transform.position + Vector3.up*2f, $"Speed: {data.MoveSpeed}");
      Handles.Label(spawner.transform.position + Vector3.up*3f, $"HP: {data.Hp}/{data.Hp}");
      SkinnedMeshRenderer mesh = data.PrefabReference.editorAsset.GetComponentInChildren<SkinnedMeshRenderer>();
      //spawner.MeshFilter.sharedMesh = sharedMesh;
      Gizmos.color = Color.red;
      Gizmos.DrawMesh(mesh.sharedMesh, spawner.transform.position, spawner.transform.rotation, mesh.transform.lossyScale);
    }
  }
}