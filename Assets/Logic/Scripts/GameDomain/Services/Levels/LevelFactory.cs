using Logic.Scripts.Services.AddressablesLoader;
using System.Threading;
using UnityEngine;

public class LevelFactory {
    private readonly IAddressablesLoaderService _addressablesLoaderService;

    public LevelFactory(IAddressablesLoaderService addressablesLoaderService) {
        _addressablesLoaderService = addressablesLoaderService;
    }

    public async Awaitable<LevelScenarioView> CreateLevelTrack(string trackAddress, CancellationTokenSource cancellationTokenSource) {
        LevelScenarioView levelTrack = (await _addressablesLoaderService.LoadAsync<LevelScenarioView>(trackAddress, cancellationTokenSource));
        return Object.Instantiate(levelTrack);
    }

    public void ReleaseTrackFromMemory(string trackAddress) {
        _addressablesLoaderService.Release(trackAddress);
    }
}
