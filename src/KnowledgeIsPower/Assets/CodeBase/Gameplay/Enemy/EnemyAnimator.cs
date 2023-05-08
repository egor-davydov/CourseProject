﻿using System;
using CodeBase.Gameplay.Logic;
using UnityEngine;

namespace CodeBase.Gameplay.Enemy
{
  public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
  {
    private static readonly int Attack = Animator.StringToHash("Attack_1");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Die = Animator.StringToHash("Die");

    private readonly int _idleStateHash = Animator.StringToHash("idle");
    private readonly int _attackStateHash = Animator.StringToHash("attack01");
    private readonly int _walkingStateHash = Animator.StringToHash("Move");
    private readonly int _deathStateHash = Animator.StringToHash("die");

    public Animator Animator { get; private set; }

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }

    private void Awake() => 
      Animator = GetComponent<Animator>();

    public void PlayHit() => Animator.SetTrigger(Hit);
    public void PlayDeath() => Animator.SetTrigger(Die);

    public void Move(float speed)
    {
      Animator.SetBool(IsMoving, true);
      Animator.SetFloat(Speed, speed);
    }

    public void StopMoving() => Animator.SetBool(IsMoving, false);

    public void PlayAttack() => Animator.SetTrigger(Attack);

    public void EnteredState(int stateHash)
    {
      State = StateFor(stateHash);
      StateEntered?.Invoke(State);
    }
    
    public void ExitedState(int stateHash) => 
      StateExited?.Invoke(StateFor(stateHash));

    private AnimatorState StateFor(int stateHash)
    {
      AnimatorState state;
      if (stateHash == _idleStateHash)
        state = AnimatorState.Idle;
      else if (stateHash == _attackStateHash)
        state = AnimatorState.Attack;
      else if (stateHash == _walkingStateHash)
        state = AnimatorState.Walking;
      else if (stateHash == _deathStateHash)
        state = AnimatorState.Died;
      else
        state = AnimatorState.Unknown;
      
      return state;
    }
  }
}