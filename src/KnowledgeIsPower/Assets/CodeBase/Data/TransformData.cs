using System;

namespace CodeBase.Data
{
  [Serializable]
  public class TransformData
  {
    public Vector3Data Position;
    public Vector3Data Rotation;
    public Vector3Data Scale;

    public TransformData(Vector3Data position, Vector3Data rotation, Vector3Data scale)
    {
      this.Position = position;
      this.Rotation = rotation;
      this.Scale = scale;
    }
  }
}