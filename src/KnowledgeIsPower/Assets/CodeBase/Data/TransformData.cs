using System;
using UnityEngine;

namespace CodeBase.Data
{
  [Serializable]
  public class TransformData
  {
    public Vector3Data position;
    public Quaternion rotation;
    public Vector3Data scale;

    public TransformData(Vector3Data position, Quaternion rotation, Vector3Data scale)
    {
      this.position = position;
      this.rotation = rotation;
      this.scale = scale;
    }
    
    public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
      this.position = position.AsVectorData();
      this.rotation = rotation;
      this.scale = scale.AsVectorData();
    }
  }
}