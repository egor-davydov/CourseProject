using System.Threading.Tasks;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Hero
{
  public interface IHeroFactory : IService
  {
    Task<GameObject> CreateHero(Vector3 at);
  }
}