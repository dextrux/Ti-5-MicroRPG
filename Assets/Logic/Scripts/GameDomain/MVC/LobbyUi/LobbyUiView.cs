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

    public void Initialize() {
        _root = _uIDocument.rootVisualElement;
        _playButton = _root.Q<Button>("play-btn");
        _customizeButton = _root.Q<Button>("customize-btn");
        _exitButton = _root.Q<Button>("exit-btn");
    }

    public void RegisterCallbacks(Action OnPlayButtonPressed, Action OnCustomizeButtonPressed, Action OnExitButtonPressed) {
        _playButton.clicked += OnPlayButtonPressed;
        _customizeButton.clicked += OnCustomizeButtonPressed;
        _exitButton.clicked += OnExitButtonPressed;
    }
}
