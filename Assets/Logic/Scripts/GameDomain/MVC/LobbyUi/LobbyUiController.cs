using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.StateMachineService;
using UnityEngine;

public class LobbyUiController : ILobbyController {
    private AbilityData _selectedAbility;

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
        _lobbyView.RegisterCallbacks(OnClickPlay, OnCustomizeClicked, OnExtiPlay, OnCustomizeExit, OnDamagePlus, OnDamageMinus,
            OnCooldownPlus, OnCooldownMinus, OnCostPlus, OnCostMinus, OnRangePlus, OnRangeMinus,
            OnCastsPlus, OnCastsMinus, OnAreaPlus, OnAreaMinus, OnAbility1Button, OnAbility2Button, OnAbility3Button, OnAbility4Button,
            OnAbility5Button, OnResetSkills);
        _selectedAbility = _abilityPointService.AllAbilities[0];
    }
    public void OnClickPlay() {
        _stateMachineService.SwitchState(_gamePlayStateFactory.Create(new GamePlayInitatorEnterData(0)));
    }
    public void OnCustomizeClicked() {
        _lobbyView.CustomizationContainer.RemoveFromClassList("close-container");
        OnAbility1Button();
    }
    public void OnCustomizeExit() {
        _lobbyView.CustomizationContainer.AddToClassList("close-container");
        _abilityPointService.SaveStats();
    }
    public void OnExtiPlay() {
        Application.Quit();
    }

    public void OnResetSkills() {
        _abilityPointService.ResetAllAbilities();
        OnAbility1Button();
    }

    public void VerifyBalanceAndSetSigns() {
        SetAllPlusSigns();
        SetAllMinusSigns();

        _lobbyView.SetUpBalanceText("Saldo: " + _abilityPointService.CurrentBalance.ToString("00"),
            "Desvantagens: " + _abilityPointService.UsedDisadvantage.ToString("00") + "/" + _abilityPointService.MaxDisadvantage.ToString("00"));

        UpdateAllAtributeText();
    }

    private void SetAllMinusSigns() {
        if (_abilityPointService.UsedDisadvantage == _abilityPointService.MaxDisadvantage) {
            _lobbyView.SetAllMinusSign(false);
        }
        else {
            _lobbyView.SetAllMinusSign(true);
        }
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Damage) > 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Damage, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cooldown) > 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Cooldown, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cost) > 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Cost, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Range) > 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Range, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Casts) > 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Casts, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Area) > 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Area, true, true);
    }

    private void SetAllPlusSigns() {
        if (_abilityPointService.CurrentBalance <= 0) {
            _lobbyView.SetAllPlusSign(false);
        }
        else {
            _lobbyView.SetAllPlusSign(true);
        }
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Damage) < 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Damage, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cooldown) < 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Cooldown, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cost) < 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Cost, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Range) < 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Range, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Casts) < 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Casts, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Area) < 0) _lobbyView.SetSignOnOff(AbilityAtributeType.Area, false, true);
    }

    public void UpdateAllAtributeText() {
        Debug.Log("SelectedAbilityNull: " + (_selectedAbility == null));
        Debug.Log("Damage: " + _selectedAbility.GetCurrentStatValue(AbilityStat.Damage).ToString("00"));
        _lobbyView.SetUpText(AbilityStat.Damage, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Damage));
        _lobbyView.SetUpText(AbilityStat.Cooldown, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Cooldown));
        _lobbyView.SetUpText(AbilityStat.Cost, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Cost));
        _lobbyView.SetUpText(AbilityStat.Range, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Range));
        _lobbyView.SetUpText(AbilityStat.Casts, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Casts));
        _lobbyView.SetUpText(AbilityStat.Area, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Area));
    }

    #region SetAbilityButtonsCallbacks
    public void OnAbility1Button() {
        _selectedAbility = _abilityPointService.AllAbilities[0];
        _lobbyView.SetAbility(_abilityPointService.AllAbilities[0]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility2Button() {
        _selectedAbility = _abilityPointService.AllAbilities[1];
        _lobbyView.SetAbility(_abilityPointService.AllAbilities[1]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility3Button() {
        _selectedAbility = _abilityPointService.AllAbilities[2];
        _lobbyView.SetAbility(_abilityPointService.AllAbilities[2]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility4Button() {
        _selectedAbility = _abilityPointService.AllAbilities[3];
        _lobbyView.SetAbility(_abilityPointService.AllAbilities[3]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility5Button() {
        _selectedAbility = _abilityPointService.AllAbilities[4];
        _lobbyView.SetAbility(_abilityPointService.AllAbilities[4]);
        VerifyBalanceAndSetSigns();
    }

    #endregion

    #region MinusPlusButtonsCallbacks
    public void OnDamagePlus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Damage)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Damage, _selectedAbility.GetDamage());
        }
    }

    public void OnDamageMinus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Damage)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Damage, _selectedAbility.GetDamage());
        }
    }

    public void OnCooldownPlus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Cooldown)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Cooldown, _selectedAbility.GetCooldown());
        }
    }

    public void OnCooldownMinus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Cooldown)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Cooldown, _selectedAbility.GetCooldown());
        }
    }

    public void OnCostPlus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Cost)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Cost, _selectedAbility.GetCost());
        }
    }

    public void OnCostMinus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Cost)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Cost, _selectedAbility.GetCost());
        }
    }

    public void OnRangePlus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Range)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Range, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Range));
        }
    }

    public void OnRangeMinus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Range)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Range, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Range));
        }
    }

    public void OnCastsPlus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Casts)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Casts, _selectedAbility.GetCasts());
        }
    }

    public void OnCastsMinus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Casts)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Casts, _selectedAbility.GetCasts());
        }
    }

    public void OnAreaPlus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Area)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Area, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Area));
        }
    }

    public void OnAreaMinus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Area)) {
            VerifyBalanceAndSetSigns();
            _lobbyView.SetUpText(AbilityStat.Area, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Area));
        }
    }
    #endregion
}
