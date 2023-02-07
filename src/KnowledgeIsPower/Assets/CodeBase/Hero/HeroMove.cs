﻿using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
  public class HeroMove : MonoBehaviour, ISavedProgress
  {
    [SerializeField] private CharacterController _characterController;

    public float MovementSpeed;

    private IInputService _inputService;

    private Camera _camera;

    public void Construct(IInputService inputService) => 
      _inputService = inputService;

    private void Start() =>
      _camera = Camera.main;

    private void Update()
    {
      Vector3 movementVector = Vector3.zero;

      if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        movementVector.y = 0;
        movementVector.Normalize();

        transform.forward = movementVector;
      }

      movementVector += Physics.gravity;

      _characterController.Move(MovementSpeed * movementVector * Time.deltaTime);
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (CurrentLevel() != progress.WorldData.PositionOnLevel.Level) return;

      Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
      if (savedPosition != null) 
        Warp(to: savedPosition);
    }

    private static string CurrentLevel() => 
      SceneManager.GetActiveScene().name;

    private void Warp(Vector3Data to)
    {
      _characterController.enabled = false;
      transform.position = to.AsUnityVector().AddY(_characterController.height);
      _characterController.enabled = true;
    }
  }
}