using System;
using System.Collections.Generic;
using CodeBase.Logic;
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
    private readonly int _walkingStateHash = Animator.StringToHash("Run");
    private readonly int _deathStateHash = Animator.StringToHash("Die");
    private Dictionary<int, AnimatorState> _states;

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    public bool IsAttacking => State == AnimatorState.Attack;
    public bool IsDefending => State == AnimatorState.Defend;


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
      Vector3 velocity = _characterController.velocity.normalized;
      float sign = SignedAngle(velocity, transform.forward);
      Debug.Log(sign);
      //_animator.SetBool(WalkBackHash, );
      _animator.SetBool(WalkBackHash, (Mathf.Abs(sign) >= 135f && Mathf.Abs(sign) <= 180|| Mathf.Abs(sign) <= 45f && Mathf.Abs(sign) >= 0) && velocity != Vector3.zero);
      _animator.SetBool(WalkLeftHash, sign > 45f && sign < 135f);
      _animator.SetBool(WalkRightHash, sign < -45f && sign > -135f);
    }

    private float SignedAngle(Vector3 from, Vector3 to)
    {
      float angle = Vector3.Angle(from, to);
      float sign = angle * Mathf.Sign(Vector3.Cross(from, to).y);

      return sign;
    }

    private Vector3 Abs(Vector3 vector3) =>
      new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));

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
  }
}