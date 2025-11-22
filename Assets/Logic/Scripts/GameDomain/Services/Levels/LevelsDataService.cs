using Logic.Scripts.Services.AddressablesLoader;
using Logic.Scripts.Services.Logger.Base;
using System.Threading;
using UnityEngine;

public class LevelsDataService : ILevelsDataService {
    private const string LEVELS_ASSET_ADRESS = "LevelsSettings";
    private const string MAX_LEVEL_NUMBER_REACHED_SAVE_KEY = "MaxLevelReachedNumber";

    public int MaxLevelNumberReached => _maxLevelNumberReached;

    private readonly IAddressablesLoaderService _addressablesLoaderService;
    //private readonly IDataPersistence _dataPersistence;
    //To-Do Corrigir DataPersistence

    private LevelsData _levelsData;
    private int _maxLevelNumberReached;

    public LevelsDataService(IAddressablesLoaderService addressablesLoaderService/*, IDataPersistence dataPersistence*/) {
        _addressablesLoaderService = addressablesLoaderService;
        //_dataPersistence = dataPersistence;
    }

    public async Awaitable LoadLevelsData(CancellationTokenSource cancellationTokenSource) {
        _levelsData = await _addressablesLoaderService.LoadAsync<LevelsData>(LEVELS_ASSET_ADRESS, cancellationTokenSource);
        //_maxLevelNumberReached = _dataPersistence.Load(MAX_LEVEL_NUMBER_REACHED_SAVE_KEY, 1);
        _maxLevelNumberReached = 0;
    }

    public LevelData[] GetAllLevelsData() {
        return _levelsData.AllLevels;
    }

    public void SetLastSavedLevel(int levelNumber) {
        LogService.LogTopic($"Set last saved level to {levelNumber}", LogTopicType.LevelsData);
        //_dataPersistence.Save(MAX_LEVEL_NUMBER_REACHED_SAVE_KEY, levelNumber);
        _maxLevelNumberReached = levelNumber;
    }

    public int GetLevelsAmount() {
        return _levelsData.AllLevels.Length;
    }

    public LevelData GetLevelData(int levelNumber) {
        Debug.Log("LevesData contagem: " + _levelsData.AllLevels.Length);
        return _levelsData.AllLevels[levelNumber];
    }
}
