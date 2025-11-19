using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;

namespace Logic.Scripts.GameDomain.Commands
{
	public class UnregisterEnvironmentActorCommand : BaseCommand, ICommandVoid
	{
		private IEnvironmentActorsRegistry _registry;
		private IEnvironmentTurnActor _actor;

		public override void ResolveDependencies()
		{
			_registry = _diContainer.Resolve<IEnvironmentActorsRegistry>();
		}

		public void SetActor(IEnvironmentTurnActor actor)
		{
			_actor = actor;
		}

		public void Execute()
		{
			if (_actor == null) return;
			_registry.Remove(_actor);
		}
	}
}

