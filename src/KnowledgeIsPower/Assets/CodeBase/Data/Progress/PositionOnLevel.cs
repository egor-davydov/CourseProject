using System;

namespace CodeBase.Data.Progress
{
  [Serializable]
  public class PositionOnLevel
  {
    public string Level;
    public Vector3Data Position;
    public Vector3Data Rotation;

    public PositionOnLevel(string level, Vector3Data position, Vector3Data rotation)
    {
      Level = level;
      Position = position;
      Rotation = rotation;
    }

    public PositionOnLevel(string initialLevel)
    {
      Level = initialLevel;
    }
  }
}