using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Random;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _random;

    private GameObject _heroGameObject;
    private IPersistentProgressService _progress;


    public GameFactory(IAssetProvider assets, IStaticDataService staticData, IRandomService random, IPersistentProgressService progress)
    {
      _assets = assets;
      _staticData = staticData;
      _random = random;
      _progress = progress;
    }

    public GameObject CreateHero(GameObject at) => 
      _heroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);

    public GameObject CreateHud()
    {
      GameObject hud = InstantiateRegistered(AssetPath.HudPath);
      hud.GetComponentInChildren<LootCounter>()
        .Construct(_progress.Progress.WorldData);
      
      return hud;
    }

    public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);
      GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);
      
      IHealth health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;
      
      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
     
      Attack attack = monster.GetComponent<Attack>();
      attack.Construct(_heroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;
      
      monster.GetComponent<AgentMoveToPlayer>()?.Construct(_heroGameObject.transform);
      monster.GetComponent<RotateToHero>()?.Construct(_heroGameObject.transform);
      
      LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.Construct(this, _random);
      lootSpawner.SetLoot(monsterData.MinLoot,monsterData.MaxLoot);

      return monster;
    }

    public LootPiece CreateLoot()
    {
      LootPiece loot = InstantiateRegistered(AssetPath.Loot)
        .GetComponent<LootPiece>();
      loot.Construct(_progress.Progress.WorldData);
      return loot;
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
    }

    public void Register(ISavedProgressReader progressReader)
    {
      if(progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);
      
      ProgressReaders.Add(progressReader);
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
    {
      GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at);
      RegisterProgressWatchers(gameObject);
      
      return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
      GameObject gameObject = _assets.Instantiate(path: prefabPath);
      RegisterProgressWatchers(gameObject);
      
      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }
  }
}