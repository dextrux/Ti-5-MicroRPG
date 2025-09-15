using Logic.Scripts.GameDomain.GameplayInitiator;
using Zenject;

public class GamePlayInstaller : MonoInstaller {
    public override void InstallBindings() {
        BindServices();
        BindControllers();
    }

    private void BindServices() {
        Container.Bind<IGamePlayInitiator>().To<GamePlayInitiator>().AsSingle().NonLazy();
    }

    private void BindControllers() {

    }
}
