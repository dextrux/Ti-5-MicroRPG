using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.StateMachineService;
using UnityEngine;

public class LobbyUiController : ILobbyController {
    private LobbyUiView _lobbyView;
    private readonly IStateMachineService _stateMachineService;
    private readonly GamePlayState.Factory _gamePlayStateFactory;
    private readonly IAudioService _audioService;

    public LobbyUiController(LobbyUiView lobbyView, IStateMachineService stateMachineService, GamePlayState.Factory gamePlayStateFactory, IAudioService audioService) {
        _lobbyView = lobbyView;
        _stateMachineService = stateMachineService;
        _gamePlayStateFactory = gamePlayStateFactory;
        _audioService = audioService;
    }

    public void InitEntryPoint() {
        _lobbyView.Initialize();
        _lobbyView.RegisterCallbacks(OnClickPlay, OnCustomizeClicked, OnExtiPlay, OnCustomizeExit, OnDamagePlus, OnDamageMinus,
            OnCooldownPlus, OnCooldownMinus, OnCostPlus, OnCostMinus, OnRangePlus, OnRangeMinus,
            OnCastsPlus, OnCastsMinus, OnAreaPlus, OnAreaMinus);
    }
    public void OnClickPlay() {
        _stateMachineService.SwitchState(_gamePlayStateFactory.Create(new GamePlayInitatorEnterData(0)));
    }
    public void OnCustomizeClicked() {
        _lobbyView.CustomizationContainer.RemoveFromClassList("close-container");
        _lobbyView.TempSetterButton1();
    }
    public void OnCustomizeExit() {
        _lobbyView.CustomizationContainer.AddToClassList("close-container");
    }
    public void OnExtiPlay() {
        Application.Quit();
    }

    public void OnDamagePlus() {

    }

    public void OnDamageMinus() {

    }

    public void OnCooldownPlus() {

    }

    public void OnCooldownMinus() {

    }

    public void OnCostPlus() {

    }

    public void OnCostMinus() {

    }

    public void OnRangePlus() {

    }

    public void OnRangeMinus() {

    }

    public void OnCastsPlus() {

    }

    public void OnCastsMinus() {

    }

    public void OnAreaPlus() {

    }

    public void OnAreaMinus() {

    }
}
