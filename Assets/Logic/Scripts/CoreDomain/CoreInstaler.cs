using Logic.Scripts.Core.Audio;
using Logic.Scripts.Core.Mvc.LoadingScreen;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.InitiatorInvokerService;
using Logic.Scripts.Services.Logger;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Core.Mvc.UICamera;
using UnityEngine;
using Zenject;
using Logic.Scripts.Services.StateMachineService;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Core.Mvc.WorldCamera;

public class CoreInstaler : MonoInstaller
{
    [SerializeField] private CoreAudioClipsScriptableObject _coreAudioClipsScriptableObject;
    [SerializeField] private UpdateSubscriptionService _updateSubscriptionService;
    [SerializeField] private AudioService _audioService;
    [SerializeField] private LoadingScreenView _loadingScreenView;
    [SerializeField] private UICameraView _uiCameraView;
    [SerializeField] private WorldCameraView _worldCameraView;
    
    public override void InstallBindings() {
        Container.BindInterfacesTo<UnityLogger>().AsSingle().NonLazy();
        Container.BindInterfacesTo<SceneLoaderService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<ResourcesLoaderService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<StateMachineService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<UpdateSubscriptionService>().FromInstance(_updateSubscriptionService).AsSingle().NonLazy();
        Container.BindInterfacesTo<AudioService>().FromInstance(_audioService).AsSingle().NonLazy();
        Container.BindInterfacesTo<SceneInitiatorsService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<CommandFactory>().AsSingle().CopyIntoAllSubContainers().NonLazy();
        Container.BindInterfacesTo<LoadingScreenController>().AsSingle().WithArguments(_loadingScreenView).NonLazy();
        Container.BindInterfacesTo<UICameraController>().AsSingle().WithArguments(_uiCameraView).NonLazy();
        Container.BindInterfacesTo<WorldCameraController>().AsSingle().WithArguments(_worldCameraView).NonLazy();
        Container.Bind<CoreAudioClipsScriptableObject>().FromScriptableObject(_coreAudioClipsScriptableObject).AsSingle().NonLazy();
        Container.Bind<GameInputActions>().AsSingle().NonLazy();
    }
}
