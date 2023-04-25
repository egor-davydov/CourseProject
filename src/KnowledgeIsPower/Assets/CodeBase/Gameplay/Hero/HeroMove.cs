using CodeBase.Data;
using CodeBase.Data.Progress;
using CodeBase.Gameplay.Hero.States;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Gameplay.Hero
{
  public class HeroMove : MonoBehaviour, ISavedProgress
  {
    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _movementSpeed;

    private IInputService _inputService;
    private Camera _camera;
    private IHeroStateMachine _hero;

    public void Construct(IInputService inputService, IHeroStateMachine heroStateMachine)
    {
      _hero = heroStateMachine;
      _inputService = inputService;
    }

    private void Start() =>
      _camera = Camera.main;

    private void Update()
    {
      Vector3 movementVector = Vector3.zero;

      if (_inputService != null && _inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        movementVector.y = 0;
        movementVector.Normalize();

        if (_hero.IsOnBasicState)
          transform.forward = movementVector;
      }

      transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
      movementVector += Physics.gravity;

      _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void ReceiveProgress(PlayerProgress progress)
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