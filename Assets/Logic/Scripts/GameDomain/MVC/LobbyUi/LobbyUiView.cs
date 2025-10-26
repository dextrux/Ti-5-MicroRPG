using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUiView : MonoBehaviour {
    [SerializeField] private UIDocument _uIDocument;
    [SerializeField] private AbilityData[] _datas;
    private VisualElement _root;
    private Button _playButton;
    private Button _customizeButton;
    private Button _exitButton;
    //Customization Infos
    private TemplateContainer _customizationContainer;
    private VisualElement _skillContainer;
    private VisualElement _pointsContainer;
    private Button _customizeExitButton;
    private Button _ability1Slot;
    private Button _ability2Slot;
    private Button _ability3Slot;
    private Button _ability4Slot;
    private Button _ability5Slot;
    private Button _ability6Slot;
    private Button _damagePlusButton;
    private Button _damageMinusButton;
    private Button _cooldownPlusButton;
    private Button _cooldownMinusButton;
    private Button _costPlusButton;
    private Button _costMinusButton;
    private Button _rangePlusButton;
    private Button _rangeMinusButton;
    private Button _castsPlusButton;
    private Button _castsMinusButton;
    private Button _areaPlusButton;
    private Button _areaMinusButton;

    #region Properties
    public TemplateContainer CustomizationContainer => _customizationContainer;
    #endregion

    public void Initialize() {
        _root = _uIDocument.rootVisualElement;
        _playButton = _root.Q<Button>("play-btn");
        _exitButton = _root.Q<Button>("exit-btn");
        _customizeButton = _root.Q<Button>("customize-btn");
        _customizationContainer = _root.Q<TemplateContainer>("customization-container");
        _customizeExitButton = _customizationContainer.Q<Button>("exit-customization-button");
        _skillContainer = _customizationContainer.Q<VisualElement>("skill-container");
        _pointsContainer = _customizationContainer.Q<VisualElement>("point-slot-container");

        _ability1Slot = _customizationContainer.Q<Button>("ability-slot1-button");
        _ability2Slot = _customizationContainer.Q<Button>("ability-slot2-button");
        _ability3Slot = _customizationContainer.Q<Button>("ability-slot3-button");
        _ability4Slot = _customizationContainer.Q<Button>("ability-slot4-button");
        _ability5Slot = _customizationContainer.Q<Button>("ability-slot5-button");
        _ability6Slot = _customizationContainer.Q<Button>("ability-slot6-button");
        _damagePlusButton = _customizationContainer.Q<Button>("damage-plus-button");
        _damageMinusButton = _customizationContainer.Q<Button>("damage-minus-button");
        _cooldownPlusButton = _customizationContainer.Q<Button>("cooldown-plus-button");
        _cooldownMinusButton = _customizationContainer.Q<Button>("cooldown-minus-button");
        _costPlusButton = _customizationContainer.Q<Button>("costs-plus-button");
        _costMinusButton = _customizationContainer.Q<Button>("costs-minus-button");
        _rangePlusButton = _customizationContainer.Q<Button>("range-plus-button");
        _rangeMinusButton = _customizationContainer.Q<Button>("range-minus-button");
        _castsPlusButton = _customizationContainer.Q<Button>("casts-plus-button");
        _castsMinusButton = _customizationContainer.Q<Button>("casts-minus-button");
        _areaPlusButton = _customizationContainer.Q<Button>("area-plus-button");
        _areaMinusButton = _customizationContainer.Q<Button>("area-minus-button");


        SetAbility(_datas[0]);
    }

    public void RegisterCallbacks(Action OnPlayButtonPressed, Action OnCustomizeButtonPressed, Action OnExitButtonPressed,
        Action OnCustomizeExitButtonPressed, Action OnDamagePlusPressed, Action OnDamageMinusPressed, Action OnCooldownPlusPressed,
        Action OnCooldownMinusPressed, Action OnCostPlusPressed, Action OnCostMinusPressed, Action OnRangePlusPressed,
        Action OnRangeMinusPressed, Action OnCastsPlusPressed, Action OnCastsMinusPressed, Action OnAreaPlusPressed,
        Action OnAreaMinusPressed) {
        _playButton.clicked += OnPlayButtonPressed;
        _customizeButton.clicked += OnCustomizeButtonPressed;
        _customizeExitButton.clicked += OnCustomizeExitButtonPressed;
        _exitButton.clicked += OnExitButtonPressed;
        /*, Action OnSetAbility1Pressed, Action OnSetAbility2Pressed, Action OnSetAbility3Pressed,
        Action OnSetAbility4Pressed, Action OnSetAbility5Pressed, Action OnSetAbility6Pressed
         * 
         * _ability1Slot.clicked += OnSetAbility1Pressed;
        _ability2Slot.clicked += OnSetAbility2Pressed;
        _ability3Slot.clicked += OnSetAbility3Pressed;
        _ability4Slot.clicked += OnSetAbility4Pressed;
        _ability5Slot.clicked += OnSetAbility5Pressed;
        _ability6Slot.clicked += OnSetAbility6Pressed;*/
        _ability1Slot.clicked += TempSetterButton1;
        _ability2Slot.clicked += TempSetterButton2;
        _ability3Slot.clicked += TempSetterButton3;
        _ability4Slot.clicked += TempSetterButton4;
        _ability5Slot.clicked += TempSetterButton5;
        _ability6Slot.clicked += TempSetterButton6;
        _damagePlusButton.clicked += OnDamagePlusPressed;
        _cooldownPlusButton.clicked += OnCooldownPlusPressed;
        _costPlusButton.clicked += OnCostPlusPressed;
        _rangePlusButton.clicked += OnRangePlusPressed;
        _castsPlusButton.clicked += OnCastsPlusPressed;
        _areaPlusButton.clicked += OnAreaPlusPressed;
        _damageMinusButton.clicked += OnDamageMinusPressed;
        _cooldownMinusButton.clicked += OnCooldownMinusPressed;
        _costMinusButton.clicked += OnCostMinusPressed;
        _rangeMinusButton.clicked += OnRangeMinusPressed;
        _castsMinusButton.clicked += OnCastsMinusPressed;
        _areaMinusButton.clicked += OnAreaMinusPressed;
    }

    public void UnregisterCallbacks() {
    }

    public void SetAbility(AbilityData data) {
        _skillContainer.dataSource = data;
        _pointsContainer.dataSource = data;
    }

    public void SetSignOff(AbilityAtributeType type, bool IsMinus) {
        switch (type) {
            case AbilityAtributeType.Damage:
                if (IsMinus) _damageMinusButton.SetEnabled(false);
                else _damagePlusButton.SetEnabled(false);
                break;
            case AbilityAtributeType.Cooldown:
                if (IsMinus) _cooldownMinusButton.SetEnabled(false);
                else _cooldownPlusButton.SetEnabled(false);
                break;
            case AbilityAtributeType.Cost:
                if (IsMinus) _costMinusButton.SetEnabled(false);
                else _costPlusButton.SetEnabled(false);
                break;
            case AbilityAtributeType.Range:
                if (IsMinus) _rangeMinusButton.SetEnabled(false);
                else _rangePlusButton.SetEnabled(false);
                break;
            case AbilityAtributeType.Casts:
                if (IsMinus) _castsMinusButton.SetEnabled(false);
                else _castsPlusButton.SetEnabled(false);
                break;
            case AbilityAtributeType.Area:
                if (IsMinus) _areaMinusButton.SetEnabled(false);
                else _areaPlusButton.SetEnabled(false);
                break;
        }
    }

    public void SetAllSign(AbilityData data) {
        if (data.Damage > 0) _damageMinusButton.SetEnabled(true);
        else _damageMinusButton.SetEnabled(false);
        if (data.Cooldown > 0) _cooldownMinusButton.SetEnabled(true);
        else _cooldownMinusButton.SetEnabled(false);
        if (data.Cost > 0) _costMinusButton.SetEnabled(true);
        else _costMinusButton.SetEnabled(false);
        if (data.Range > 0) _rangeMinusButton.SetEnabled(true);
        else _rangeMinusButton.SetEnabled(false);
        if (data.Casts > 0) _castsMinusButton.SetEnabled(true);
        else _castsMinusButton.SetEnabled(false);
        if (data.Area > 0) _areaMinusButton.SetEnabled(true);
        else _areaMinusButton.SetEnabled(false);
    }

    public void TempSetterButton1() {
        SetAbility(_datas[0]);
        SetAllSign(_datas[0]);
    }

    public void TempSetterButton2() {
        SetAbility(_datas[1]);
        SetAllSign(_datas[1]);
    }

    public void TempSetterButton3() {
        SetAbility(_datas[2]);
        SetAllSign(_datas[2]);
    }

    public void TempSetterButton4() {
        SetAbility(_datas[3]);
        SetAllSign(_datas[3]);
    }

    public void TempSetterButton5() {
        SetAbility(_datas[4]);
        SetAllSign(_datas[4]);
    }

    public void TempSetterButton6() {
        SetAbility(_datas[5]);
        SetAllSign(_datas[5]);
    }
}
