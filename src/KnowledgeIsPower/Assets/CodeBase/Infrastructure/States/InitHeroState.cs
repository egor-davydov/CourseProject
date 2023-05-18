using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Infrastructure.Factories.Hero;
using CodeBase.Infrastructure.Factories.Hud;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Level;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class InitHeroState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IHeroStateMachine _heroStateMachine;
    private readonly HeroProvider _heroProvider;
    private readonly IStaticDataService _staticData;
    private readonly IHeroFactory _heroFactory;
    private readonly IHudFactory _hudFactory;

    public InitHeroState(
      GameStateMachine gameStateMachine,
      IHeroStateMachine heroStateMachine,
      HeroProvider heroProvider,
      IStaticDataService staticData,
      IHeroFactory heroFactory,
      IHudFactory hudFactory
    )
    {
      _gameStateMachine = gameStateMachine;
      _heroStateMachine = heroStateMachine;
      _heroProvider = heroProvider;
      _staticData = staticData;
      _heroFactory = heroFactory;
      _hudFactory = hudFactory;
    }

    public async void Enter()
    {
      GameObject hero = await InitHero();
      await InitHud(hero);
      CameraFollow(hero);
      
      _gameStateMachine.Enter<FinishLevelInitializationState>();
    }

    public void Exit()
    {
    }

    private async Task<GameObject> InitHero()
    {
      GameObject heroObject = await _heroFactory.CreateHero(LevelStaticData().InitialHeroPosition);
      _heroStateMachine.Initialize(heroObject);
      _heroProvider.Initialize(heroObject);
      _heroStateMachine.Enter(HeroStateType.Basic);

      return heroObject;
    }

    private async Task InitHud(GameObject hero)
    {
      GameObject hud = await _hudFactory.CreateHud();

      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }
    
    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);
    
    private void CameraFollow(GameObject hero) =>
      Camera.main.GetComponent<CameraFollow>().Follow(hero);
  }
}