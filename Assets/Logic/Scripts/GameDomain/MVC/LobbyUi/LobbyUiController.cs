using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.StateMachineService;
using UnityEngine;

public class LobbyUiController : ILobbyController {
    private readonly LobbyUiView _lobbyView;
    private readonly IStateMachineService _stateMachineService;
    private readonly GamePlayState.Factory _gamePlayStateFactory;
    private readonly IAudioService _audioService;
    private readonly IAbilityPointService _abilityPointService;

    public LobbyUiController(LobbyUiView lobbyView, IStateMachineService stateMachineService, GamePlayState.Factory gamePlayStateFactory,
        IAudioService audioService, IAbilityPointService abilityPointService) {
        _lobbyView = lobbyView;
        _stateMachineService = stateMachineService;
        _gamePlayStateFactory = gamePlayStateFactory;
        _audioService = audioService;
        _abilityPointService = abilityPointService;
    }

    public void InitEntryPoint() {
        _lobbyView.Initialize(_abilityPointService.AllAbilities[0]);
        _lobbyView.RegisterCallbacks(OnClickPlay, OnClickLoad, OnClickOptions, OnExitPlay);
    }

    public void OnClickPlay() {
        _stateMachineService.SwitchState(_gamePlayStateFactory.Create(new GamePlayInitatorEnterData(0)));
    }

    public void OnClickLoad() {

    }

    public void OnClickOptions() {

    }

    public void OnExitPlay() {
        Application.Quit();
    }
}