using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
  public interface IStaticDataService : IService
  {
    void LoadMonsters();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
  }
}