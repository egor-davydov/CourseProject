using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private IInputService _inputService;

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private GameObject HeroGameObject { get; set; }

    public GameFactory(IAssetProvider assets, IStaticDataService staticData, IInputService inputService)
    {
      _assets = assets;
      _staticData = staticData;
      _inputService = inputService;
    }

    public GameObject CreateHero(GameObject at)
    {
      HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);

      var heroMove = HeroGameObject.GetComponent<HeroMove>();
      heroMove.Construct(_inputService);
      heroMove.MovementSpeed = _staticData.HeroData.MovementSpeed;
      
      return HeroGameObject;
    }

    public GameObject CreateHud() =>
      InstantiateRegistered(AssetPath.HudPath);

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
    }

    public void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }

    public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);
      GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

      var health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
      
      var attack = monster.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.AttackEffectiveDistance;
      attack.Damage = monsterData.Damage;

      monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

      return monster;
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
      {
        Register(progressReader);
      }
    }
  }
}