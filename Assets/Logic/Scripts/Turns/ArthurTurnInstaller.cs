using Zenject;

namespace Logic.Scripts.Turns
{
    public class ArthurTurnInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TurnEventBus>().AsSingle();

            Container.BindInterfacesAndSelfTo<ActionPointsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EchoService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BossActionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnviromentActionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TurnFlowController>().AsSingle();

            Container.BindInterfacesAndSelfTo<BarrierToggleRule>().AsSingle();
            Container.BindInterfacesAndSelfTo<HazardZoneToggleRule>().AsSingle();

        }
    }
}
