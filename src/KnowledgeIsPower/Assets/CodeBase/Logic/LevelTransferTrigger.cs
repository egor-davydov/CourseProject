﻿using System;
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    public string TransferTo;
    
    private const string PlayerTag = "Player";
    
    private IGameStateMachine _stateMachine;
    
    private bool _triggered;

    private void Awake() => 
      _stateMachine = AllServices.Container.Single<IGameStateMachine>();

    private void OnTriggerEnter(Collider other)
    {
      if (_triggered)
        return;
      
      if (other.CompareTag(PlayerTag))
      {
        _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        _triggered = true;
      }
    }
  }
}