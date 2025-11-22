using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.AddressablesLoader {
    public interface IAddressablesLoaderService {
        Awaitable<T> LoadAsync<T>(string address, CancellationTokenSource cancellationTokenSource) where T : Object;
        void Release(string address);
        void ReleaseAll();
        bool IsLoaded(string address);
    }
}