using System;
using UnityEngine;

namespace CodeBase.Data
{
  [Serializable]
  public class BoxColliderData
  {
    public Vector3 Size;
    public Vector3 Center;

    public BoxColliderData(Vector3 size, Vector3 center)
    {
      Size = size;
      Center = center;
    }
  }
}