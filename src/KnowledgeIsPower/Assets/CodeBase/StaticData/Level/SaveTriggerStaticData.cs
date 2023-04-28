using System;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class SaveTriggerStaticData
  {
    public string Id;
    public TransformData TransformData;
    public BoxColliderData BoxColliderData;
    public Vector3 FirePosition;

    public SaveTriggerStaticData(string id, TransformData transformData, BoxColliderData boxColliderData, Vector3 firePosition)
    {
      Id = id;
      TransformData = transformData;
      BoxColliderData = boxColliderData;
      FirePosition = firePosition;
    }
  }
}