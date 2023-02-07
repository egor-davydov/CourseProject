using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
  public interface IStaticDataService : IService
  {
    void LoadData();
    GameObject ForMonster(MonsterTypeId typeId);
  }
}