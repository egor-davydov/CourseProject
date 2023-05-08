using System;
using System.Collections;
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
    private bool _isRotating;

    public void Construct(Transform heroTransform)
      => _heroTransform = heroTransform;

    public void Initialize(float rotationSpeed) => 
      _rotationSpeed = rotationSpeed;

    private void Start() =>
      GetComponent<IHealth>().OnTakeDamage += StartRotationCoroutine;

    private void Update()
    {
      if (_isRotating)
        transform.SmoothLookAt(_heroTransform, _rotationSpeed * Time.deltaTime);
    }

    private void StartRotationCoroutine() => 
      StartCoroutine(StartRotation());

    private IEnumerator StartRotation()
    {
      _isRotating = true;
      yield return new WaitForSeconds(_rotationSpeed);
      _isRotating = false;
    }
  }
}