using System;

namespace CodeBase.Gameplay.Logic
{
  public interface IHealth
  {
    event Action HealthChanged;
    event Action OnTakeDamage;
    float Current { get; set; }
    float Max { get; set; }
    void TakeDamage(float damage);
  }
}