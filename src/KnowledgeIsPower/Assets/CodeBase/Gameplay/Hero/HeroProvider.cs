using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Gameplay.Hero
{
  public class HeroProvider : IService
  {
    public GameObject HeroObject { get; private set; }

    public void Initialize(GameObject heroObject) => 
      HeroObject = heroObject;
  }
}