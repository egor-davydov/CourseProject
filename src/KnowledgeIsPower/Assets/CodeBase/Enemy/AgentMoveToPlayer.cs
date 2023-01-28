using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
  public class AgentMoveToPlayer : MonoBehaviour
  {
    private const float MinimalDistance = 1;

    public NavMeshAgent Agent;

    private Transform _heroTransform;
    private IGameFactory _gameFactory;

    private void Start()
    {
      _gameFactory = AllServices.Container.Single<IGameFactory>();

      if (_gameFactory.HeroGameObject != null)
        InitializeHero();
      else
        _gameFactory.HeroCreated += HeroCreated;
    }

    private void Update()
    {
      if (Initialized() && HeroNotReached()) 
        Agent.destination = _heroTransform.position;
    }

    private void HeroCreated() => 
      InitializeHero();

    private void InitializeHero() => 
      _heroTransform = _gameFactory.HeroGameObject.transform;

    private bool Initialized() => 
      _heroTransform != null;

    private bool HeroNotReached() => 
      Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
  }
}