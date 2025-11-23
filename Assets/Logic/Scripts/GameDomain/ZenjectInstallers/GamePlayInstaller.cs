using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.GameplayInitiator;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Boss;
using Logic.Scripts.Turns;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Zenject;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.GameDomain.MVC.Echo;

public class GamePlayInstaller : MonoInstaller {

    [SerializeField] private NaraView _naraViewPrefab;
    [SerializeField] private NaraConfigurationSO _naraConfiguration;

    [SerializeField] private GamePlayUiView _gamePlayUiView;

    [SerializeField] private AbilityData[] _skills;

    [SerializeField] private LayerMask _layerMaskMouse;

    public override void InstallBindings() {
        BindServices();
        BindControllers();
    }

    private void BindServices() {
        Container.Bind<IGamePlayInitiator>().To<GamePlayInitiator>().AsSingle().NonLazy();
        Container.BindInterfacesTo<LevelCancellationTokenService>().AsSingle().NonLazy();
        Container.Bind<INaraMovementControllerFactory>().To<NaraMovementControllerFactory>().AsSingle();
        Container.BindInterfacesTo<GamePlayDataService>().AsSingle().NonLazy();
    }

    private void BindControllers() {
        Container.BindInterfacesTo<GamePlayUiController>().AsSingle().WithArguments(_gamePlayUiView).NonLazy();
        Container.BindInterfacesTo<LevelScenarioController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<NaraController>().AsSingle().WithArguments(_naraViewPrefab, _naraConfiguration).NonLazy();
        Container.BindInterfacesTo<GameInputActionsController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<CastController>().AsSingle().WithArguments(_skills).NonLazy();
        //Container.BindInterfacesTo<EchoController>().AsSingle().WithArguments(_echoviewPrefab).NonLazy();
        Container.BindInterfacesTo<PortalController>().AsSingle().NonLazy();

        /*if (_bossPhases != null) Container.BindInstance(_bossPhases);
        Container.BindInterfacesTo<BossAbilityController>().AsSingle().WithArguments((BossBehaviorSO)null).NonLazy();
        Container.BindInterfacesTo<BossController>().AsSingle().WithArguments(_bossViewPrefab, _bossConfiguration, _bossPhases).NonLazy();
        Container.BindInterfacesAndSelfTo<BossActionService>().AsSingle().NonLazy();*/
    }
}
