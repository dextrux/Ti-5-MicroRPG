using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUiView : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;
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
    }

    public void RegisterCallbacks(Action OnPlayButtonPressed, Action OnCustomizeButtonPressed, Action OnExitButtonPressed,
        Action OnCustomizeExitButton) {
        _playButton.clicked += OnPlayButtonPressed;
        _customizeButton.clicked += OnCustomizeButtonPressed;
        _customizeExitButton.clicked += OnCustomizeExitButton;
        _exitButton.clicked += OnExitButtonPressed;
    }

    public void UnregisterCallbacks() {
    }

    public void SetAbility(AbilityData data) {
        _skillContainer.dataSource = data;
        _pointsContainer.dataSource = data;
    }
}
