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

    [SerializeField] private BossView _bossViewPrefab;
    [SerializeField] private BossConfigurationSO _bossConfiguration;
    [SerializeField] private BossBehaviorSO _bossBehavior;

    [SerializeField] private GamePlayUiView _gamePlayUiView;

    [SerializeField] private AbilityData[] _skillSet1;
    [SerializeField] private AbilityData[] _skillSet2;
    [SerializeField] private AbilityData[] _skillSet3;

    [SerializeField] private EchoView _echoviewPrefab;

    [SerializeField] private LayerMask _layerMaskMouse;

    public override void InstallBindings() {
        BindServices();
        BindControllers();
    }

    private void BindServices() {
        Container.Bind<IGamePlayInitiator>().To<GamePlayInitiator>().AsSingle().NonLazy();
    }

    private void BindControllers() {
        Container.BindInterfacesTo<NaraController>().AsSingle().WithArguments(_naraViewPrefab, _naraConfiguration).NonLazy();
        //Container.BindInterfacesTo<LevelCancellationTokenService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<GameInputActionsController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<GamePlayUiController>().AsSingle().WithArguments(_gamePlayUiView).NonLazy();
        Container.BindInterfacesTo<CastController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<AbilityController>().AsSingle().WithArguments(_skillSet1, _skillSet2, _skillSet3).NonLazy();
        Container.BindInterfacesTo<EchoController>().AsSingle().WithArguments(_echoviewPrefab).NonLazy();

        Container.BindInstance(_bossBehavior);
        Container.BindInterfacesTo<BossAbilityController>().AsSingle().WithArguments(_bossBehavior).NonLazy();
        Container.BindInterfacesTo<BossController>().AsSingle().WithArguments(_bossViewPrefab, _bossConfiguration, _bossBehavior).NonLazy();
        Container.BindInterfacesAndSelfTo<BossActionService>().AsSingle().NonLazy();
    }
}
