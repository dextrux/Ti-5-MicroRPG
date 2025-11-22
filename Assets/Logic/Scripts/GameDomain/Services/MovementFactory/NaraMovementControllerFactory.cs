using System;
using Zenject;

public interface INaraMovementControllerFactory {
    NaraMovementController Create(Type movementControllerType, params object[] extraArgs);
}

public class NaraMovementControllerFactory : INaraMovementControllerFactory {
    readonly DiContainer _container;

    public NaraMovementControllerFactory(DiContainer container) {
        _container = container;
    }

    public NaraMovementController Create(Type movementControllerType, params object[] extraArgs) {
        return (NaraMovementController)_container.Instantiate(movementControllerType, extraArgs);
    }
}
