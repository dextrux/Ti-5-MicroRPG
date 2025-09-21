using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.GameplayInitiator;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Boss;
using Logic.Scripts.Turns;
using Zenject;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.GameDomain.MVC.Abilitys;

public class GamePlayInstaller : MonoInstaller {

    [SerializeField] private NaraView _naraViewPrefab;
    [SerializeField] private NaraConfigurationSO _naraConfiguration;

    [SerializeField] private BossView _bossViewPrefab;
    [SerializeField] private BossConfigurationSO _bossConfiguration;

    [SerializeField] private GamePlayUiView _gamePlayUiView;

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
        //Container.BindInterfacesTo<GamePlayUiController>().AsSingle().WithArguments(_gamePlayUiView).NonLazy();
        Container.BindInterfacesTo<AbilityController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<BossController>().AsSingle().WithArguments(_bossViewPrefab, _bossConfiguration).NonLazy();
        Container.BindInterfacesAndSelfTo<BossActionService>().AsSingle().NonLazy();
    }
}
