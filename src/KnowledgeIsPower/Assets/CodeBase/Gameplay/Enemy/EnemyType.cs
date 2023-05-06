using CodeBase.StaticData.Monster;
using UnityEngine;

namespace CodeBase.Gameplay.Enemy
{
  public class EnemyType : MonoBehaviour
  {
    [SerializeField]
    private MonsterTypeId _monsterType;

    public MonsterTypeId Value => _monsterType;
  }
}