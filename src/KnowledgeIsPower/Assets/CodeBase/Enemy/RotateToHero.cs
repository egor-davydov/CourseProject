using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class RotateToHero : Follow
  {
    public float Speed;
    
    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private Vector3 _positionToLook;

    private void Start()
    {
      _gameFactory = AllServices.Container.Single<IGameFactory>();


      if (HeroAlreadyExists())
        InitializeHero();
      else
        _gameFactory.HeroCreated += InitializeHero;
    }

    private void Update()
    {
      if (Initialized())
        RotateTowardsHero();
    }

    private bool HeroAlreadyExists() =>
      _gameFactory.HeroGameObject != null;

    private void RotateTowardsHero()
    {
      UpdatePositionToLook();

      transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
    }

    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
      Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

    private float SpeedFactor() => 
      Speed * Time.deltaTime;

    private Quaternion TargetRotation(Vector3 positionToLook) => 
      Quaternion.LookRotation(positionToLook);

    private void UpdatePositionToLook()
    {
      Vector3 positionDifference = _heroTransform.position - transform.position;
      _positionToLook = new Vector3(positionDifference.x, transform.position.y, positionDifference.z);
    }

    private bool Initialized() =>
      _heroTransform != null;

    private void InitializeHero() =>
      _heroTransform = _gameFactory.HeroGameObject.transform;
  }
}