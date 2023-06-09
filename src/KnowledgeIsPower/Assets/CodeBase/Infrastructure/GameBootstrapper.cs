﻿using CodeBase.Gameplay.Hero.States;
using CodeBase.Gameplay.Logic;
using CodeBase.Infrastructure.States;
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
      GameStateMachine gameStateMachine = new GameStateMachine(new SceneLoader(this), Instantiate(CurtainPrefab), new AllServices(), _heroStateMachine);
      gameStateMachine.Enter<BootstrapState>();

      DontDestroyOnLoad(this);
    }
  }
}