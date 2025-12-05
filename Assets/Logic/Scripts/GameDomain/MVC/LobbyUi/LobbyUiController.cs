using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.StateMachineService;
using UnityEngine;

public class LobbyUiController : ILobbyController {
    private readonly LobbyUiView _lobbyView;
    private readonly IStateMachineService _stateMachineService;
    private readonly ExplorationState.Factory _explorationStateFactory;
    private readonly IAudioService _audioService;
    private readonly IAbilityPointService _abilityPointService;

    public LobbyUiController(LobbyUiView lobbyView, IStateMachineService stateMachineService, ExplorationState.Factory explorationStateFactory,
        IAudioService audioService, IAbilityPointService abilityPointService) {
        _lobbyView = lobbyView;
        _stateMachineService = stateMachineService;
        _explorationStateFactory = explorationStateFactory;
        _audioService = audioService;
        _abilityPointService = abilityPointService;
    }

    public void InitEntryPoint() {
        _lobbyView.Initialize(_abilityPointService.AllAbilities[0]);
        _lobbyView.RegisterCallbacks(OnClickPlay, OnClickLoad, OnClickOptions, OnExitPlay);
    }

    public void OnClickPlay() {
        _stateMachineService.SwitchState(_explorationStateFactory.Create(new ExplorationInitiatorEnterData(0)));
    }

    public void OnClickLoad() {

    }

    public void OnClickOptions() {

    }

    public void OnExitPlay() {
        Application.Quit();
    }
}