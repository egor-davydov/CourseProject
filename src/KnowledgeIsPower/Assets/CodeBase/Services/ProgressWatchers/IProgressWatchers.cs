using System.Collections.Generic;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.ProgressWatchers
{
  public interface IProgressWatchers : IService
  {
    void Register(GameObject gameObject);
    List<IProgressReader> Readers { get; }
    List<IProgressWriter> Writers { get; }
  }
}