using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Gameplay.Logic;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroAnimator : MonoBehaviour, IAnimationStateReader
  {
    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    public Animator _animator;

    private static readonly int VerticalMoveHash = Animator.StringToHash("VerticalMove");
    private static readonly int HorizontalMoveHash = Animator.StringToHash("HorizontalMove");
    private static readonly int FastAttackHash = Animator.StringToHash("AttackNormal");
    private static readonly int LongAttackHash = Animator.StringToHash("AttackSpecial");
    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int WalkRightHash = Animator.StringToHash("WalkRight");
    private static readonly int WalkLeftHash = Animator.StringToHash("WalkLeft");
    private static readonly int WalkBackHash = Animator.StringToHash("WalkBack");
    private static readonly int DefendHash = Animator.StringToHash("Defend");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private readonly int _idleStateHash = Animator.StringToHash("Idle");
    private readonly int _idleStateFullHash = Animator.StringToHash("Base Layer.Idle");
    private readonly int _fastAttackStateHash = Animator.StringToHash("Attack Normal");
    private readonly int _longAttackStateHash = Animator.StringToHash("Attack Special");
    private readonly int _defendStateHash = Animator.StringToHash("Defend");
    private readonly int _runStateHash = Animator.StringToHash("Run");
    private readonly int _walkingStateHash = Animator.StringToHash("WalkBack");
    private readonly int _deathStateHash = Animator.StringToHash("Die");
    
    private Dictionary<int, AnimatorState> _states;
    private IHeroStateMachine _hero;

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    public bool IsAttacking => State == AnimatorState.Attack;
    public bool IsDefending => State == AnimatorState.Defend;

    public void Construct(IHeroStateMachine heroStateMachine) => 
      _hero = heroStateMachine;

    private void Awake()
    {
      _states = new Dictionary<int, AnimatorState>
      {
        [_idleStateHash] = AnimatorState.Idle,
        [_fastAttackStateHash] = AnimatorState.Attack,
        [_longAttackStateHash] = AnimatorState.Attack,
        [_defendStateHash] = AnimatorState.Defend,
        [_walkingStateHash] = AnimatorState.Walking,
      };
    }

    private void Update()
    {
      if(_hero == null)
        return;
      
      Vector3 velocityNormalized = _characterController.velocity.normalized;
      if(_hero.IsOnBasicState)
      {
        _animator.SetBool(RunHash, IsMoving(velocityNormalized));
        return;
      }
      
      AnimateFocusedState(velocityNormalized);
    }

    public void TurnOffRun() =>
      _animator.SetBool(RunHash, false);

    public void TurnOffFocusedAnimations()
    {
      _animator.SetBool(WalkBackHash, false);
      _animator.SetBool(WalkLeftHash, false);
      _animator.SetBool(WalkRightHash, false);
    }

    public void PlayHit() =>
      _animator.SetTrigger(HitHash);

    public void PlayFastAttack() =>
      _animator.SetTrigger(FastAttackHash);

    public void PlayLongAttack() =>
      _animator.SetTrigger(LongAttackHash);

    public void PlayDefend() =>
      _animator.SetTrigger(DefendHash);

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
    
    private void AnimateFocusedState(Vector3 velocityNormalized)
    {
      float angle = AngleBetween(velocityNormalized, transform.forward);

      _animator.SetBool(WalkBackHash,
        IsMoving(velocityNormalized) && (
          MovingBack(angle)
          || MovingForward(angle))
      );

      _animator.SetBool(WalkLeftHash, MovingLeft(angle));

      _animator.SetBool(WalkRightHash, MovingRight(angle));
    }

    private float AngleBetween(Vector3 from, Vector3 to)
    {
      float angle = Vector3.Angle(from, to);
      float signedAngle = angle * Mathf.Sign(Vector3.Cross(from, to).y);

      return signedAngle;
    }

    private bool IsMoving(Vector3 velocityNormalized) =>
      velocityNormalized != Vector3.zero;

    private bool MovingBack(float angle) =>
      Mathf.Abs(angle) >= 135f && Mathf.Abs(angle) <= 180;

    private bool MovingForward(float angle) =>
      Mathf.Abs(angle) <= 45f && Mathf.Abs(angle) >= 0;

    private bool MovingLeft(float angle) =>
      angle > 45f && angle < 135f;

    private bool MovingRight(float angle) =>
      angle < -45f && angle > -135f;
  }
}