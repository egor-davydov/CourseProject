﻿using UnityEngine;

namespace CodeBase.Gameplay.Hero.States
{
  public class HeroBasicState : IHeroState
  {
    private GameObject _heroObject;

    public void Initialize(GameObject heroObject) => 
      _heroObject = heroObject;

    public void Enter()
    {
      //Debug.Log($"{Time.time} HeroBasicState");
      _heroObject.GetComponent<HeroAnimator>().TurnOffFocusedAnimations();
    }

    public void Exit()
    {
    }
  }
}