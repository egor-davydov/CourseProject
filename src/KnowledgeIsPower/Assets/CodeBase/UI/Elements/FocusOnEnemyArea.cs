using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
  public class FocusOnEnemyArea : MonoBehaviour
  {
    [SerializeField]
    private Button _focusOnEnemyButton;

    [SerializeField]
    private Button _changeEnemyLeftButton;

    [SerializeField]
    private Button _changeEnemyRightButton;

    private IHeroStateMachine _heroStateMachine;
    private GameObject _heroObject;
    private Image _focusButtonImage;
    private FocusSphere _focusSphere;
    private HeroFocusOnEnemy _heroFocusOnEnemy;

    public void Initialize(IHeroStateMachine heroStateMachine, GameObject heroObject)
    {
      _heroObject = heroObject;
      _heroStateMachine = heroStateMachine;

      _focusSphere = _heroObject.GetComponentInChildren<FocusSphere>();
      _heroFocusOnEnemy = _heroObject.GetComponent<HeroFocusOnEnemy>();
    }

    private void Awake()
    {
      _focusOnEnemyButton.onClick.AddListener(FocusOnEnemy);
      _changeEnemyLeftButton.onClick.AddListener(ChangeEnemyLeft);
      _changeEnemyRightButton.onClick.AddListener(ChangeEnemyRight);

      _focusButtonImage = _focusOnEnemyButton.GetComponent<Image>();
    }

    private void Update()
    {
      bool canChangeFocus = _focusSphere.EnemiesInSphere.Count > 1 && _heroStateMachine.IsFocused;
      _changeEnemyLeftButton.gameObject.SetActive(canChangeFocus);
      _changeEnemyRightButton.gameObject.SetActive(canChangeFocus);
      if(_focusSphere.EnemiesInSphere.Count <= 0)
        EnterBasicState();
    }

    private void FocusOnEnemy()
    {
      if (_heroStateMachine.IsOnBasicState)
      {
        if (_focusSphere.EnemiesInSphere.Count <= 0)
          return;

        EnterFocusedState();
      }
      else
        EnterBasicState();
    }

    private void EnterFocusedState()
    {
      _heroStateMachine.Enter(HeroStateType.Focused);
      _focusButtonImage.color = new Color(1, 0, 0, 0.5f);
    }

    private void EnterBasicState()
    {
      _heroStateMachine.Enter(HeroStateType.Basic);
      _focusButtonImage.color = new Color(1, 1, 1, 0.5f);
    }

    private void ChangeEnemyLeft() =>
      _heroFocusOnEnemy.ChangeEnemyToFocusLeft();

    private void ChangeEnemyRight()
    {
      Debug.Log("Pressed");
      _heroFocusOnEnemy.ChangeEnemyToFocusRight();
    }
  }
}