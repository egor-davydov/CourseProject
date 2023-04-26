using System.Collections.Generic;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.ProgressWatchers
{
  public class ProgressWatchers : IProgressWatchers
  {
    public List<IProgressReader> Readers { get; } = new List<IProgressReader>();
    public List<IProgressWriter> Writers { get; } = new List<IProgressWriter>(); 

    public void Register(GameObject gameObject)
    {
      foreach (IProgressReader progressReader in gameObject.GetComponentsInChildren<IProgressReader>()) 
        Readers.Add(progressReader);
      foreach (IProgressWriter progressWriter in gameObject.GetComponentsInChildren<IProgressWriter>()) 
        Writers.Add(progressWriter);
    }
  }
}