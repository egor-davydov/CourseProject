using System.Threading.Tasks;
using CodeBase.Gameplay.Enemy.Loot;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Loot
{
  public class LootFactory : ILootFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IProgressWatchers _progressWatchers;
    private readonly IPersistentProgressService _persistentProgressService;

    public LootFactory(IAssetProvider assets, IProgressWatchers progressWatchers, IPersistentProgressService persistentProgressService)
    {
      _assets = assets;
      _progressWatchers = progressWatchers;
      _persistentProgressService = persistentProgressService;
    }

    public async Task<LootPiece> CreateLoot()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
      LootPiece lootPiece = Object.Instantiate(prefab).GetComponent<LootPiece>();
      _progressWatchers.Register(lootPiece.gameObject);

      lootPiece.Construct(_persistentProgressService.Progress.WorldData.LootData);

      return lootPiece;
    }
  }
}