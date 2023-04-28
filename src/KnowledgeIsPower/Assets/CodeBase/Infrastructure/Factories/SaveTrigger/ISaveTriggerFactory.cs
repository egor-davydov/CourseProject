using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.SaveTrigger
{
  public interface ISaveTriggerFactory : IService
  {
    Task<GameObject> CreateSaveTrigger(string id, TransformData transformData, BoxColliderData boxColliderData, Vector3 firePosition);
  }
}