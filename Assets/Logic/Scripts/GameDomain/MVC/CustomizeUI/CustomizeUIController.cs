using Logic.Scripts.GameDomain.MVC.Abilitys;

public class CustomizeUIController : ICustomizeUIController {
    private readonly CustomizeUIView _customizationView;
    private readonly IAbilityPointService _abilityPointService;
    private AbilityData _selectedAbility;

    public CustomizeUIController(CustomizeUIView customizationView, IAbilityPointService abilityPointService) {
        _customizationView = customizationView;
        _abilityPointService = abilityPointService;
    }

    public void InitEntryPoint() {
        //_selectedAbility = _abilityPointService.AllAbilities[0];
        //_customizationView.InitStartPoint(_abilityPointService.AllAbilities[0]);
        //HideCustomize();
        //_customizationView.RegisterCallbacks(HideCustomize, OnDamagePlus, OnDamageMinus, OnCooldownPlus, OnCooldownMinus, OnCostPlus,
        //    OnCostMinus, OnRangePlus, OnRangeMinus, OnAbility1Button, OnAbility2Button, OnAbility3Button, OnAbility4Button, OnAbility5Button);
    }

    public void ShowCustomize() {
        _customizationView.ShowCustomize();
    }

    public void HideCustomize() {
        _customizationView.HideCustomize();
    }


    public void VerifyBalanceAndSetSigns() {
        SetAllPlusSigns();
        SetAllMinusSigns();

        _customizationView.SetUpBalanceText(_abilityPointService.CurrentBalance.ToString("00") + "/" + _abilityPointService.Advantage.ToString("00"));

        UpdateAllAtributeText();
    }

    private void SetAllMinusSigns() {
        if (_abilityPointService.UsedDisadvantage == _abilityPointService.MaxDisadvantage) {
            _customizationView.SetAllMinusSign(false);
        }
        else {
            _customizationView.SetAllMinusSign(true);
        }
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Damage) > 0) _customizationView.SetSignOnOff(AbilityStat.Damage, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cooldown) > 0) _customizationView.SetSignOnOff(AbilityStat.Cooldown, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cost) > 0) _customizationView.SetSignOnOff(AbilityStat.Cost, true, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Range) > 0) _customizationView.SetSignOnOff(AbilityStat.Range, true, true);
    }

    private void SetAllPlusSigns() {
        if (_abilityPointService.CurrentBalance <= 0) {
            _customizationView.SetAllPlusSign(false);
        }
        else {
            _customizationView.SetAllPlusSign(true);
        }
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Damage) < 0) _customizationView.SetSignOnOff(AbilityStat.Damage, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cooldown) < 0) _customizationView.SetSignOnOff(AbilityStat.Cooldown, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Cost) < 0) _customizationView.SetSignOnOff(AbilityStat.Cost, false, true);
        if (_selectedAbility.GetModifierStatValue(AbilityStat.Range) < 0) _customizationView.SetSignOnOff(AbilityStat.Range, false, true);
    }

    public void UpdateAllAtributeText() {
        _customizationView.SetUpText(AbilityStat.Damage, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Damage));
        _customizationView.SetUpText(AbilityStat.Cooldown, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Cooldown));
        _customizationView.SetUpText(AbilityStat.Cost, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Cost));
        _customizationView.SetUpText(AbilityStat.Range, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Range));
    }

    #region SetAbilityButtonsCallbacks
    public void OnAbility1Button() {
        _selectedAbility = _abilityPointService.AllAbilities[0];
        _customizationView.SetAbility(_abilityPointService.AllAbilities[0]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility2Button() {
        _selectedAbility = _abilityPointService.AllAbilities[1];
        _customizationView.SetAbility(_abilityPointService.AllAbilities[1]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility3Button() {
        _selectedAbility = _abilityPointService.AllAbilities[2];
        _customizationView.SetAbility(_abilityPointService.AllAbilities[2]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility4Button() {
        _selectedAbility = _abilityPointService.AllAbilities[3];
        _customizationView.SetAbility(_abilityPointService.AllAbilities[3]);
        VerifyBalanceAndSetSigns();
    }

    public void OnAbility5Button() {
        _selectedAbility = _abilityPointService.AllAbilities[4];
        _customizationView.SetAbility(_abilityPointService.AllAbilities[4]);
        VerifyBalanceAndSetSigns();
    }

    #endregion

    #region MinusPlusButtonsCallbacks
    public void OnDamagePlus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Damage)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Damage, _selectedAbility.GetDamage());
        }
    }

    public void OnDamageMinus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Damage)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Damage, _selectedAbility.GetDamage());
        }
    }

    public void OnCooldownPlus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Cooldown)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Cooldown, _selectedAbility.GetCooldown());
        }
    }

    public void OnCooldownMinus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Cooldown)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Cooldown, _selectedAbility.GetCooldown());
        }
    }

    public void OnCostPlus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Cost)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Cost, _selectedAbility.GetCost());
        }
    }

    public void OnCostMinus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Cost)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Cost, _selectedAbility.GetCost());
        }
    }

    public void OnRangePlus() {
        if (_abilityPointService.TryIncreaseStat(_selectedAbility, AbilityStat.Range)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Range, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Range));
        }
    }

    public void OnRangeMinus() {
        if (_abilityPointService.TryDecreaseStat(_selectedAbility, AbilityStat.Range)) {
            VerifyBalanceAndSetSigns();
            _customizationView.SetUpText(AbilityStat.Range, (int)_selectedAbility.GetCurrentStatValue(AbilityStat.Range));
        }
    }
    #endregion
}
