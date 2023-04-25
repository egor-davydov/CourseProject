using CodeBase.Gameplay.Hero;
using CodeBase.Gameplay.Hero.States;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
  public class FocusOnEnemyButton : MonoBehaviour
  {
    [SerializeField]
    private Button _button;

    private IHeroStateMachine _heroStateMachine;
    private FocusSphere _focusSphere;

    public void Initialize(IHeroStateMachine heroStateMachine, FocusSphere focusSphere)
    {
      _focusSphere = focusSphere;
      _heroStateMachine = heroStateMachine;
    }

    private void Awake() =>
      _button.onClick.AddListener(FocusOnEnemy);

    private void FocusOnEnemy()
    {
      if (_heroStateMachine.IsOnBasicState)
      {
        if (_focusSphere.EnemiesInSphere.Count > 0)
          _heroStateMachine.Enter(HeroStateType.Focused);
        _button.GetComponent<Image>().color = new Color(1, 0, 0, 0.5f);
      }
      else
      {
        _heroStateMachine.Enter(HeroStateType.Basic);
        _button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
      }
    }
  }
}