using System.Threading.Tasks;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Hud
{
  public interface IHudFactory : IService
  {
    Task<GameObject> CreateHud();
  }
}