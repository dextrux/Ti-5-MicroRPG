using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.Services.CommandFactory;

public class StartLevelCommand : BaseCommand, ICommandVoid {
    IWorldCameraController _iWorldCameraController;
    
    public override void ResolveDependencies() {
        _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
    }

    public void Execute() {
        //_iWorldCameraController.StartFollowTarget(); Adicionar o playerController para liberar
    }

}
