using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUiView : MonoBehaviour {
    [SerializeField] private UIDocument _uIDocument;
    private VisualElement _root;
    private Button _playButton;
    private Button _customizeButton;
    private Button _exitButton;

    private Label _balanceLabel;
    private Label _disadvantageLabel;

    private Label _damageLabel;
    private Label _cooldownLabel;
    private Label _costLabel;
    private Label _rangeLabel;

    private TemplateContainer _customizationContainer;
    private VisualElement _skillContainer;
    private VisualElement _pointsContainer;
    private Button _customizeExitButton;
    private Button _resetSkillsButton;
    private Button _ability1Slot;
    private Button _ability2Slot;
    private Button _ability3Slot;
    private Button _ability4Slot;
    private Button _ability5Slot;
    private Button _damagePlusButton;
    private Button _damageMinusButton;
    private Button _cooldownPlusButton;
    private Button _cooldownMinusButton;
    private Button _costPlusButton;
    private Button _costMinusButton;
    private Button _rangePlusButton;
    private Button _rangeMinusButton;

    #region Properties
    public TemplateContainer CustomizationContainer => _customizationContainer;
    #endregion

    public void Initialize(AbilityData data) {
        _root = _uIDocument.rootVisualElement;
        _playButton = _root.Q<Button>("play-btn");
        _exitButton = _root.Q<Button>("exit-btn");
        _customizeButton = _root.Q<Button>("customize-btn");
        _customizationContainer = _root.Q<TemplateContainer>("customization-container");
        _customizeExitButton = _customizationContainer.Q<Button>("exit-customization-button");
        _resetSkillsButton = _customizationContainer.Q<Button>("reset-button");
        _skillContainer = _customizationContainer.Q<VisualElement>("skill-container");
        _pointsContainer = _customizationContainer.Q<VisualElement>("point-slot-container");

        _balanceLabel = _customizationContainer.Q<Label>("balance-txt");
        _disadvantageLabel = _customizationContainer.Q<Label>("disadvantage-txt");

        _damageLabel = _customizationContainer.Q<Label>("damage-txt");
        _cooldownLabel = _customizationContainer.Q<Label>("cooldown-txt");
        _costLabel = _customizationContainer.Q<Label>("cost-txt");
        _rangeLabel = _customizationContainer.Q<Label>("range-txt");

        _ability1Slot = _customizationContainer.Q<Button>("ability-slot1-button");
        _ability2Slot = _customizationContainer.Q<Button>("ability-slot2-button");
        _ability3Slot = _customizationContainer.Q<Button>("ability-slot3-button");
        _ability4Slot = _customizationContainer.Q<Button>("ability-slot4-button");
        _ability5Slot = _customizationContainer.Q<Button>("ability-slot5-button");
        _damagePlusButton = _customizationContainer.Q<Button>("damage-plus-button");
        _damageMinusButton = _customizationContainer.Q<Button>("damage-minus-button");
        _cooldownPlusButton = _customizationContainer.Q<Button>("cooldown-plus-button");
        _cooldownMinusButton = _customizationContainer.Q<Button>("cooldown-minus-button");
        _costPlusButton = _customizationContainer.Q<Button>("costs-plus-button");
        _costMinusButton = _customizationContainer.Q<Button>("costs-minus-button");
        _rangePlusButton = _customizationContainer.Q<Button>("range-plus-button");
        _rangeMinusButton = _customizationContainer.Q<Button>("range-minus-button");

        SetAbility(data);
    }

    public void RegisterCallbacks(Action OnPlayButtonPressed, Action OnCustomizeButtonPressed, Action OnExitButtonPressed,
        Action OnCustomizeExitButtonPressed, Action OnDamagePlusPressed, Action OnDamageMinusPressed, Action OnCooldownPlusPressed,
        Action OnCooldownMinusPressed, Action OnCostPlusPressed, Action OnCostMinusPressed, Action OnRangePlusPressed,
        Action OnRangeMinusPressed, Action OnSetAbility1Pressed, Action OnSetAbility2Pressed, Action OnSetAbility3Pressed,
        Action OnSetAbility4Pressed, Action OnSetAbility5Pressed, Action OnResetSkillPressed) {
        _playButton.clicked += OnPlayButtonPressed;
        _customizeButton.clicked += OnCustomizeButtonPressed;
        _customizeExitButton.clicked += OnCustomizeExitButtonPressed;
        _exitButton.clicked += OnExitButtonPressed;
        _ability1Slot.clicked += OnSetAbility1Pressed;
        _ability2Slot.clicked += OnSetAbility2Pressed;
        _ability3Slot.clicked += OnSetAbility3Pressed;
        _ability4Slot.clicked += OnSetAbility4Pressed;
        _ability5Slot.clicked += OnSetAbility5Pressed;
        _damagePlusButton.clicked += OnDamagePlusPressed;
        _cooldownPlusButton.clicked += OnCooldownPlusPressed;
        _costPlusButton.clicked += OnCostPlusPressed;
        _rangePlusButton.clicked += OnRangePlusPressed;
        _damageMinusButton.clicked += OnDamageMinusPressed;
        _cooldownMinusButton.clicked += OnCooldownMinusPressed;
        _costMinusButton.clicked += OnCostMinusPressed;
        _rangeMinusButton.clicked += OnRangeMinusPressed;
        _resetSkillsButton.clicked += OnResetSkillPressed;
    }

    public void UnregisterCallbacks() {
    }

    public void SetAbility(AbilityData data) {
        _skillContainer.dataSource = data;
        _pointsContainer.dataSource = data;
    }

    public void SetSignOnOff(AbilityStat type, bool isMinus, bool newState) {
        switch (type) {
            case AbilityStat.Damage:
                if (isMinus) _damageMinusButton.SetEnabled(newState);
                else _damagePlusButton.SetEnabled(newState);
                break;
            case AbilityStat.Cooldown:
                if (isMinus) _cooldownMinusButton.SetEnabled(newState);
                else _cooldownPlusButton.SetEnabled(newState);
                break;
            case AbilityStat.Cost:
                if (isMinus) _costMinusButton.SetEnabled(newState);
                else _costPlusButton.SetEnabled(newState);
                break;
            case AbilityStat.Range:
                if (isMinus) _rangeMinusButton.SetEnabled(newState);
                else _rangePlusButton.SetEnabled(newState);
                break;
        }
    }

    public void SetAllMinusSign(AbilityData data) {
        if (data.GetModifierStatValue(AbilityStat.Damage) > 1) _damageMinusButton.SetEnabled(true);
        else _damageMinusButton.SetEnabled(false);
        if (data.GetModifierStatValue(AbilityStat.Cooldown) > 1) _cooldownMinusButton.SetEnabled(true);
        else _cooldownMinusButton.SetEnabled(false);
        if (data.GetModifierStatValue(AbilityStat.Cost) > 1) _costMinusButton.SetEnabled(true);
        else _costMinusButton.SetEnabled(false);
        if (data.GetModifierStatValue(AbilityStat.Range) > 1) _rangeMinusButton.SetEnabled(true);
        else _rangeMinusButton.SetEnabled(false);
    }

    public void SetUpText(AbilityStat type, int newValue) {
        switch (type) {
            case AbilityStat.Damage:
                Debug.Log("Damage: " + newValue.ToString("00"));
                _damageLabel.text = newValue.ToString("00");
                break;
            case AbilityStat.Cooldown:
                Debug.Log("Cooldown: " + newValue.ToString("00"));
                _cooldownLabel.text = newValue.ToString("00");
                break;
            case AbilityStat.Cost:
                Debug.Log("Cost: " + newValue.ToString("00"));
                _costLabel.text = newValue.ToString("00");
                break;
            case AbilityStat.Range:
                Debug.Log("Range: " + newValue.ToString("00"));
                _rangeLabel.text = newValue.ToString("00");
                break;
        }
    }

    public void SetAllMinusSign(bool newState) {
        _damageMinusButton.SetEnabled(newState);
        _cooldownMinusButton.SetEnabled(newState);
        _costMinusButton.SetEnabled(newState);
        _rangeMinusButton.SetEnabled(newState);
    }

    public void SetAllPlusSign(bool newState) {
        _damagePlusButton.SetEnabled(newState);
        _cooldownPlusButton.SetEnabled(newState);
        _costPlusButton.SetEnabled(newState);
        _rangePlusButton.SetEnabled(newState);
    }

    public void SetUpBalanceText(string balanceText, string disadvantageText) {
        _balanceLabel.text = balanceText;
        _disadvantageLabel.text = disadvantageText;
    }
}
