using System.Threading.Tasks;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Services;

namespace CodeBase.Infrastructure.Factories.Loot
{
  public interface ILootFactory : IService
  {
    Task<LootPiece> CreateLoot();
  }
}