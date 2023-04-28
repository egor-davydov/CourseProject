using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Gameplay.Logic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories.EnemySpawner;
using CodeBase.Infrastructure.Factories.Hero;
using CodeBase.Infrastructure.Factories.Hud;
using CodeBase.Infrastructure.Factories.LevelTransfer;
using CodeBase.Infrastructure.Factories.Loot;
using CodeBase.Infrastructure.Factories.SaveTrigger;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class GameStateMachine : IGameStateMachine
  {
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services, HeroStateMachine heroStateMachine)
    {
      _states = new Dictionary<Type, IExitableState>
      {
        [typeof(BootstrapState)] = new BootstrapState(this, heroStateMachine, sceneLoader, services),

        [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistentProgressService>(),
          services.Single<ISaveLoadService>()),

        [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain,
          services.Single<IAssetProvider>(), services.Single<IProgressWatchers>()),

        [typeof(InitLevelState)] = new InitLevelState(this, services.Single<IStaticDataService>(),
          services.Single<IPersistentProgressService>(), services.Single<IEnemySpawnerFactory>(),
          services.Single<ISaveTriggerFactory>(), services.Single<ILevelTransferFactory>(),
          services.Single<ILootFactory>(), services.Single<IUIFactory>(), services.Single<IRespawnService>()),

        [typeof(InitHeroState)] = new InitHeroState(this, services.Single<IHeroStateMachine>(), services.Single<HeroProvider>(),
          services.Single<IStaticDataService>(), services.Single<IHeroFactory>(), services.Single<IHudFactory>()),

        [typeof(FinishLevelInitializationState)] = new FinishLevelInitializationState(this, loadingCurtain,
          services.Single<IPersistentProgressService>(), services.Single<IProgressWatchers>()),

        [typeof(GameLoopState)] = new GameLoopState(this),
      };
    }

    public void Enter<TState>() where TState : class, IState
    {
      IState state = ChangeState<TState>();
      state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
      TState state = ChangeState<TState>();
      state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
      _activeState?.Exit();

      TState state = GetState<TState>();
      _activeState = state;
      //Debug.Log(state);
      
      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState =>
      _states[typeof(TState)] as TState;
  }
}