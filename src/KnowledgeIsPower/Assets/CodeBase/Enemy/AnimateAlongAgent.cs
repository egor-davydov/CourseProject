using System;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(NavMeshAgent))]
  [RequireComponent(typeof(EnemyAnimator))]
  public class AnimateAlongAgent : MonoBehaviour
  {
    private const float MinimumVelocity = 0.1f;
    
    public EnemyAnimator EnemyAnimator;
    public NavMeshAgent Agent;


    private void Update()
    {
      if(ShouldMove())
        EnemyAnimator.Move(Agent.velocity.magnitude);
      else
        EnemyAnimator.StopMoving();
    }

    private bool ShouldMove() => 
      Agent.velocity.magnitude > MinimumVelocity && Agent.remainingDistance > Agent.radius;
  }
}