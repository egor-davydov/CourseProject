using System.Threading.Tasks;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.LevelTransfer
{
  public interface ILevelTransferFactory : IService
  {
    Task CreateLevelTransfer(Vector3 at);
  }
}