using System;
using System.Collections.Generic;

namespace CodeBase.Data.Progress
{
  [Serializable]
  public class SaveTriggersData
  {
    public List<string> UsedSaveTriggers = new List<string>();
  }
}