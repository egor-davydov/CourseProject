using System.Threading.Tasks;
using CodeBase.Services;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Enemy
{
  public interface IEnemyFactory : IService
  {
    Task<GameObject> CreateEnemy(MonsterTypeId typeId, Transform parent);
  }
}