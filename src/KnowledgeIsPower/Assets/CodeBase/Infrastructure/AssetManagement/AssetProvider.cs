using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
    private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

    public async Task<T> Load<T>(AssetReference assetReference)
    {
      if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
        return (T)completedHandle.Result;
      
      AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
      handle.Completed += operationHandle =>
      {
        _completedCache[assetReference.AssetGUID] = operationHandle;
      };
      
      AddHandle(assetReference.AssetGUID, handle);

      return await handle.Task;
    }

    public void CleanUp()
    {
      foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
      foreach (AsyncOperationHandle handle in resourceHandles)
        Addressables.Release(handle);
    }
    
    public GameObject Instantiate(string path, Vector3 at)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab, at, Quaternion.identity);
    }

    public GameObject Instantiate(string path)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab);
    }

    private void AddHandle(string key, AsyncOperationHandle handle)
    {
      if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
      {
        resourceHandles = new List<AsyncOperationHandle>();
        _handles[key] = resourceHandles;
      }

      resourceHandles.Add(handle);
    }
  }
}