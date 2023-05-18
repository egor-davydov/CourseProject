using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Extensions;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Services.Input;
using CodeBase.Services.ProgressWatchers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Gameplay.Hero
{
  public class HeroMove : MonoBehaviour, IProgressWriter, IProgressReader
  {
    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private AudioSource _heroAudioSource;

    [SerializeField]
    private AudioClip[] _heroSteps;

    private float MovementSpeed =>
      _hero.IsOnBasicState
        ? _stats.BasicMovementSpeed
        : _stats.FocusedMovementSpeed;

    private IInputService _inputService;
    private Camera _camera;
    private IHeroStateMachine _hero;
    private Stats _stats;
    private int _previousStepNumber;

    public void Construct(IInputService inputService, IHeroStateMachine heroStateMachine)
    {
      _hero = heroStateMachine;
      _inputService = inputService;
    }

    private void Start() =>
      _camera = Camera.main;

    private void Update()
    {
      if (_hero == null || _stats == null || _inputService == null)
        return;

      Vector3 movementVector = Vector3.zero;

      if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        movementVector.y = 0;
        movementVector.Normalize();

        if (_hero.IsOnBasicState)
          transform.forward = movementVector;
      }

      transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
      movementVector += Physics.gravity;

      _characterController.Move(MovementSpeed * movementVector * Time.deltaTime);
    }

    private void OnStep()
    {
      int currentStepNumber = _previousStepNumber != _heroSteps.Length - 1
        ? _previousStepNumber + 1
        : 0;
      _heroAudioSource.PlayOneShot(_heroSteps[currentStepNumber]);
      _previousStepNumber = currentStepNumber;
    }
    
    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(
        CurrentLevel(),
        transform.position.AsVectorData(),
        transform.rotation.AsVectorData()
      );
    }

    public void ReceiveProgress(PlayerProgress progress)
    {
      _stats = progress.HeroStats;
      if (CurrentLevel() != progress.WorldData.PositionOnLevel.Level)
        return;

      Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
      Vector3Data savedRotation = progress.WorldData.PositionOnLevel.Rotation;
      if (savedPosition != null && savedRotation != null)
      {
        Warp(to: savedPosition);
        Rotate(on: savedRotation);
      }
    }

    private static string CurrentLevel() =>
      SceneManager.GetActiveScene().name;

    private void Warp(Vector3Data to)
    {
      _characterController.enabled = false;
      transform.position = to.AsUnityVector().AddY(_characterController.height);
      _characterController.enabled = true;
    }

    private void Rotate(Vector3Data on) =>
      transform.rotation = on.AsUnityQuaternion();
  }
}