using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
  public class SpawnMarker : MonoBehaviour
  {
    [SerializeField]
    private MonsterTypeId _monsterTypeId;

    public MonsterTypeId MonsterTypeId => _monsterTypeId;
  }
}