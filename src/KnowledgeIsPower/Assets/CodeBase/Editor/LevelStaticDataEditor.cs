using System.Linq;
using CodeBase.Data;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Logic;
using CodeBase.Gameplay.Logic.EnemySpawners;
using CodeBase.Gameplay.Logic.Save;
using CodeBase.StaticData.Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData)target;

      if (GUILayout.Button("Collect"))
        Collect(levelData);

      EditorUtility.SetDirty(target);
    }

    public static void Collect(LevelStaticData levelData)
    {
      if (levelData == null)
        return;

      levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
        .Select(x => new EnemySpawnerStaticData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, new TransformData(
          x.transform.position.AsVectorData(),
          x.transform.rotation.AsVectorData(),
          x.transform.localScale.AsVectorData()
        ))).ToList();

      levelData.SaveTriggers = FindObjectsOfType<SaveMarker>()
        .Select(x => new SaveTriggerStaticData(x.GetComponent<UniqueId>().Id, new TransformData(
          x.transform.position.AsVectorData(),
          x.transform.rotation.AsVectorData(),
          x.transform.localScale.AsVectorData()
        ), new BoxColliderData(x.GetComponent<BoxCollider>().size, x.GetComponent<BoxCollider>().center))).ToList();

      levelData.LevelKey = SceneManager.GetActiveScene().name;
      levelData.InitialHeroPosition = GameObject.FindWithTag(Tags.InitialPointTag).transform.position;
      levelData.LevelTransfer.Position = GameObject.FindWithTag(Tags.LevelTransferInitialPointTag).transform.position;
    }
  }
}