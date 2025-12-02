using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using UnityEngine;

public class LoadExplorationStateCommand : BaseCommand, ICommandAsync {
    private IAudioService _audioService;
    private INaraController _naraController;
    //private GamePlayAudioClipsScriptableObject _gamePlayAudioClipsScriptableObject; Lista de audios espec�ficos da exploracao
    private ICommandFactory _commandFactory;

    private ExplorationInitiatorEnterData _enterData;

    public LoadExplorationStateCommand SetEnterData(ExplorationInitiatorEnterData enterData) {
        _enterData = enterData;
        return this;
    }

    public override void ResolveDependencies() {
        _audioService = _diContainer.Resolve<IAudioService>();
        //_gamePlayAudioClipsScriptableObject = _diContainer.Resolve<GamePlayAudioClipsScriptableObject>();
        _naraController = _diContainer.Resolve<INaraController>();
        _commandFactory = _diContainer.Resolve<ICommandFactory>();
    }

    public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        await _commandFactory.CreateCommandAsync<LoadLevelCommand>().SetEnterData(new LoadLevelCommandData(_enterData.LevelNumberToEnter)).Execute(cancellationTokenSource);
        return;
    }
}
