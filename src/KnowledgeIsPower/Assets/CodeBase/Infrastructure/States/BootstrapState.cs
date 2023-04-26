using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Factories.Enemy;
using CodeBase.Infrastructure.Factories.EnemySpawner;
using CodeBase.Infrastructure.Factories.Hero;
using CodeBase.Infrastructure.Factories.Hud;
using CodeBase.Infrastructure.Factories.LevelTransfer;
using CodeBase.Infrastructure.Factories.Loot;
using CodeBase.Services;
using CodeBase.Services.Ads;
using CodeBase.Services.IAP;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ProgressWatchers;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class BootstrapState : IState
  {
    private const string Initial = "Initial";
    
    private readonly GameStateMachine _stateMachine;
    private readonly HeroStateMachine _heroStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, HeroStateMachine heroStateMachine, SceneLoader sceneLoader, AllServices services)
    {
      _stateMachine = stateMachine;
      _heroStateMachine = heroStateMachine;
      _sceneLoader = sceneLoader;
      _services = services;

      RegisterServices();
    }

    public void Enter() =>
      _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);

    public void Exit()
    {
    }

    private void RegisterServices()
    {
      RegisterStaticDataService();
      RegisterAdsService();
      RegisterAssetProvider();

      _services.RegisterSingle<HeroProvider>(new HeroProvider());
      _services.RegisterSingle<IGameStateMachine>(_stateMachine);
      _services.RegisterSingle<IHeroStateMachine>(_heroStateMachine);
      _services.RegisterSingle<IInputService>(InputService());
      _services.RegisterSingle<IRandomService>(new RandomService());
      _services.RegisterSingle<IRespawnService>(new RespawnService());
      _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
      _services.RegisterSingle<IProgressWatchers>(new ProgressWatchers());
      
      RegisterIAPService(new IAPProvider(),
        _services.Single<IPersistentProgressService>(),
        _services.Single<IRespawnService>()
        );
    
      _services.RegisterSingle<IUIFactory>(new UIFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IStaticDataService>(),
        _services.Single<IPersistentProgressService>(),
        _services.Single<IAdsService>(),
        _services.Single<IIAPService>()
        ));
      
      _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));

      _services.RegisterSingle<ILootFactory>(new LootFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IProgressWatchers>(),
        _services.Single<IPersistentProgressService>()
      ));
      _services.RegisterSingle<ILevelTransferFactory>(new LevelTransferFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IProgressWatchers>(),
        _services.Single<IGameStateMachine>()
      ));
      _services.RegisterSingle<IHeroFactory>(new HeroFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IProgressWatchers>(),
        _services.Single<IHeroStateMachine>(),
        _services.Single<IInputService>()
      ));
      _services.RegisterSingle<IHudFactory>(new HudFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IProgressWatchers>(),
        _services.Single<IHeroStateMachine>(),
        _services.Single<HeroProvider>(),
        _services.Single<IWindowService>()
        ));
      _services.RegisterSingle<IEnemyFactory>(new EnemyFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IProgressWatchers>(),
        _services.Single<IStaticDataService>(),
        _services.Single<HeroProvider>(),
        _services.Single<IRandomService>(),
        _services.Single<ILootFactory>()
        ));
      _services.RegisterSingle<IEnemySpawnerFactory>(new EnemySpawnerFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IProgressWatchers>(),
        _services.Single<IEnemyFactory>()
        ));
      
      _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
        _services.Single<IPersistentProgressService>(),
        _services.Single<IProgressWatchers>()));
    }

    private void RegisterAssetProvider()
    {
      AssetProvider assetProvider = new AssetProvider();
      _services.RegisterSingle<IAssetProvider>(assetProvider);
      assetProvider.Initialize();
    }

    private void RegisterAdsService()
    {
      IAdsService adsService = new AdsService();
      adsService.Initialize();
      _services.RegisterSingle<IAdsService>(adsService);
    }
    private void RegisterIAPService(IAPProvider iapProvider, IPersistentProgressService progress, IRespawnService respawnService)
    {
      IAPService iapService = new IAPService(iapProvider, progress, respawnService);
      iapService.Initialize();
      _services.RegisterSingle<IIAPService>(iapService);
    }

    private void RegisterStaticDataService()
    {
      IStaticDataService staticData = new StaticDataService();
      staticData.Load();
      _services.RegisterSingle(staticData);
    }

    private void EnterLoadLevel() =>
      _stateMachine.Enter<LoadProgressState>();

    private static IInputService InputService() =>
      Application.isEditor
        ? (IInputService) new StandaloneInputService()
        : new MobileInputService();
  }
}