using System;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    public LoadingCurtain CurtainPrefab;
    private HeroStateMachine _heroStateMachine;

    private void Awake()
    {
      _heroStateMachine = new HeroStateMachine();
      _heroStateMachine.Enter(HeroStateType.Basic);
      GameStateMachine gameStateMachine = new GameStateMachine(new SceneLoader(this), Instantiate(CurtainPrefab), AllServices.Container, _heroStateMachine);
      gameStateMachine.Enter<BootstrapState>();

      DontDestroyOnLoad(this);
    }

    private void Update() => 
      _heroStateMachine.OnUpdate();
  }
}