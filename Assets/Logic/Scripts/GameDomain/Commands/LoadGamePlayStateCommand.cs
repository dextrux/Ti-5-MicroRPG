using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using UnityEngine;

namespace Logic.Scripts.GameDomain.Commands {
    public class LoadGamePlayStateCommand : BaseCommand, ICommandAsync {
        private IAudioService _audioService;
        private INaraController _naraController;
        //private GamePlayAudioClipsScriptableObject _gamePlayAudioClipsScriptableObject; Lista de audios espec�ficos do gameplay
        private ICommandFactory _commandFactory;

        private GamePlayInitatorEnterData _enterData;

        public LoadGamePlayStateCommand SetEnterData(GamePlayInitatorEnterData enterData) {
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
            await _commandFactory.CreateCommandAsync<LoadLevelCommand>()
                .SetEnterData(new LoadLevelCommandData(_enterData.LevelNumberToEnter))
                .SetBoss(_enterData.LevelNumberToEnter)
                .Execute(cancellationTokenSource);
            return;
        }
    }
}