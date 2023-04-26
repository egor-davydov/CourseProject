using System;
using CodeBase.Data.Progress;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroDefend : MonoBehaviour, ISavedProgressReader
  {
    [SerializeField]
    private HeroAnimator Animator;

    private IInputService _inputService;
    private Stats _stats;
    
    public float MaximumDamageToBlock => _stats.MaximumDamageToBlock;
    public float DefendFactor => _stats.DefendFactor;
    public event Action Activate; 
    public event Action Deactivate; 
    
    public void Construct(IInputService inputService) =>
      _inputService = inputService;

    private void Update()
    {
      if (_inputService != null && _inputService.IsDefendButtonUp())
        Animator.PlayDefend();
    }

    private void OnDefend() => 
      Activate?.Invoke();

    private void OnDefendEnd() => 
      Deactivate?.Invoke();

    public void ReceiveProgress(PlayerProgress progress) => 
      _stats = progress.HeroStats;
  }
}