using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
  [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
  public class LevelStaticData : ScriptableObject
  {
    public string LevelKey;
    public List<EnemySpawnerStaticData> EnemySpawners;
    public List<SaveTriggerStaticData> SaveTriggers;
    public LevelTransferStaticData LevelTransfer;
    public Vector3 InitialHeroPosition;
  }
}