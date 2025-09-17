using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.Services.CommandFactory;
using UnityEngine;

namespace Logic.Scripts.GameDomain.Commands {
    public class UnlockCameraInvokedCommand : BaseCommand, ICommandVoid {
        IWorldCameraController _iWorldCameraController;
        public override void ResolveDependencies() {
            _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
        }

        public void Execute() {
            _iWorldCameraController.UnlockCameraRotate();
        }
    }
}