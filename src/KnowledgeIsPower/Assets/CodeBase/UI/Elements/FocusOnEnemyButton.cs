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
      Debug.Log(_focusSphere.EnemiesInSphere.Count);
      if (_focusSphere.EnemiesInSphere.Count > 0)
        _heroStateMachine.Enter(HeroStateType.Focused);
    }
  }
}