using System.Linq;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    private const string InitialPointTag = "InitialPoint";
    private const string LevelTransferInitialPointTag = "LevelTransferInitialPoint";
    
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData) target;

      if (GUILayout.Button("Collect")) 
        Collect(levelData);

      EditorUtility.SetDirty(target);
    }

    public static void Collect(LevelStaticData levelData)
    {
      levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
        .Select(x => new EnemySpawnerStaticData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, new TransformData(x.transform.position, x.transform.rotation, x.transform.localScale)))
        .ToList();

      levelData.LevelKey = SceneManager.GetActiveScene().name;

      levelData.InitialHeroPosition = GameObject.FindWithTag(InitialPointTag).transform.position;

      levelData.LevelTransfer.Position = GameObject.FindWithTag(LevelTransferInitialPointTag).transform.position;
    }
  }
}