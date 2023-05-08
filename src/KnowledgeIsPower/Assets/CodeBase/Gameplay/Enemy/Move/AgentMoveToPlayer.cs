using CodeBase.Data;
using CodeBase.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Enemy.Move
{
  public class AgentMoveToPlayer : Follow
  {
    public NavMeshAgent Agent;

    private const float MinimalDistance = 1;

    private Transform _heroTransform;

    public void Construct(Transform heroTransform) =>
      _heroTransform = heroTransform;

    private void Update()
    {
      if (_heroTransform && IsHeroNotReached())
        Agent.destination = _heroTransform.position;
    }

    private bool IsHeroNotReached() =>
      Agent.transform.position.SqrMagnitudeTo(_heroTransform.position) >= MinimalDistance;
  }
}