using CodeBase.Extensions.GameplayExtensions;
using CodeBase.Gameplay.Logic;
using UnityEngine;

namespace CodeBase.Gameplay.Enemy.Move
{
  public class RotateOnDamage : MonoBehaviour
  {
    [SerializeField]
    private float _rotationSpeed;

    private Transform _heroTransform;

    public void Construct(Transform heroTransform)
      => _heroTransform = heroTransform;

    private void Start() => 
      GetComponent<IHealth>().OnTakeDamage += RotateToHero;

    private void RotateToHero() =>
      transform.SmoothLookAt(_heroTransform, _rotationSpeed);
  }
}