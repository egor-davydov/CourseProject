using System.Threading.Tasks;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.LevelTransfer
{
  public interface ILevelTransferFactory : IService
  {
    Task<GameObject> CreateLevelTransfer(Vector3 at, string transferTo);
  }
}