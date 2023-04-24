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

    public void Initialize() => 
      Addressables.InitializeAsync();

    public async Task<T> Load<T>(AssetReference assetReference) where T : class
    {
      if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
        return await completedHandle.Convert<T>().Task;

      AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference.AssetGUID);
      _completedCache[assetReference.AssetGUID] = handle;

      return await handle.Task;
    }

    public async Task<T> Load<T>(string address) where T : class
    {
      if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
        return await completedHandle.Convert<T>().Task;

      AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
      _completedCache[address] = handle;

      return await handle.Task;
    }

    public Task<GameObject> Instantiate(string address, Vector3 at) =>
      Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

    public Task<GameObject> Instantiate(string address) =>
      Addressables.InstantiateAsync(address).Task;

    public Task<GameObject> Instantiate(string address, Transform under) =>
      Addressables.InstantiateAsync(address, under).Task;

    public void Cleanup()
    {
      foreach (AsyncOperationHandle handle in _completedCache.Values)
        Addressables.Release(handle);

      _completedCache.Clear();
    }
  }
}