﻿using System;
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

    public SaveTriggerStaticData(string id, TransformData transformData, BoxColliderData boxColliderData)
    {
      Id = id;
      TransformData = transformData;
      BoxColliderData = boxColliderData;
    }
  }
}