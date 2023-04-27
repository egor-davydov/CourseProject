using CodeBase.StaticData.Monster;
using UnityEngine;

namespace CodeBase.Gameplay.Logic.EnemySpawners
{
  public class SpawnMarker : MonoBehaviour
  {
    [SerializeField]
    private MonsterTypeId _monsterTypeId;

    public MonsterTypeId MonsterTypeId => _monsterTypeId;
  }
}