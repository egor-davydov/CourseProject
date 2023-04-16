using System;
using System.Collections.Generic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroAnimator : MonoBehaviour, IAnimationStateReader
  {
    [SerializeField] private CharacterController _characterController;
    [SerializeField] public Animator _animator;

    private static readonly int MoveHash = Animator.StringToHash("Walking");
    private static readonly int FastAttackHash = Animator.StringToHash("AttackNormal");
    private static readonly int LongAttackHash = Animator.StringToHash("AttackSpecial");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private readonly int _idleStateHash = Animator.StringToHash("Idle");
    private readonly int _idleStateFullHash = Animator.StringToHash("Base Layer.Idle");
    private readonly int _fastAttackStateHash = Animator.StringToHash("Attack Normal");
    private readonly int _longAttackStateHash = Animator.StringToHash("Attack Special");
    private readonly int _walkingStateHash = Animator.StringToHash("Run");
    private readonly int _deathStateHash = Animator.StringToHash("Die");
    private Dictionary<int, AnimatorState> _states;

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    public bool IsAttacking => State == AnimatorState.Attack ;

    private void Awake()
    {
      _states = new Dictionary<int, AnimatorState>
      {
        [_idleStateHash] = AnimatorState.Idle,
        [_fastAttackStateHash] = AnimatorState.Attack,
        [_longAttackStateHash] = AnimatorState.Attack,
        [_walkingStateHash] = AnimatorState.Walking,
      };
    }

    private void Update() => 
      _animator.SetFloat(MoveHash, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);

    public void PlayHit() => 
      _animator.SetTrigger(HitHash);

    public void PlayFastAttack() => 
      _animator.SetTrigger(FastAttackHash);

    public void PlayLongAttack() => 
      _animator.SetTrigger(LongAttackHash);

    public void PlayDeath() => 
      _animator.SetTrigger(DieHash);

    public void ResetToIdle() => 
      _animator.Play(_idleStateHash, -1);

    public void EnteredState(int stateHash)
    {
      State = _states[stateHash];
      StateEntered?.Invoke(State);
    }

    public void ExitedState(int stateHash) => 
      StateExited?.Invoke(_states[stateHash]);
  }
}