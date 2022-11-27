using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Net;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssets
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completeCache = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public void Initialize() => 
            Addressables.InitializeAsync();

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completeCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completeHandle))
                return completeHandle.Result as T;

            return await RunWithCasheOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference), 
                casheKey: assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completeCache.TryGetValue(address, out AsyncOperationHandle completeHandle))
                return completeHandle.Result as T;

            return await RunWithCasheOnComplete(
                Addressables.LoadAssetAsync<T>(address), 
                casheKey: address);
        }

        public Task<GameObject> Instantiate(string address) =>
            Addressables.InstantiateAsync(address).Task;

        public Task<GameObject> Instantiate(string address, Vector3 at) =>
            Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandlers in _handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandlers)
                    Addressables.Release(handle);

            _completeCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWithCasheOnComplete<T>(AsyncOperationHandle<T> handle, string casheKey) where T : class
        {
            handle.Completed += completeHandle =>
            {
                _completeCache[casheKey] = completeHandle;
            };

            AddHandle(casheKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandlers))
            {
                resourceHandlers = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandlers;
            }

            resourceHandlers.Add(handle);
        }

    }
}
