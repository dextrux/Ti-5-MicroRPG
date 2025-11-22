namespace Logic.Scripts.Turns
{
	public class EnvironmentActorsRegistry : IEnvironmentActorsRegistry
	{
		private readonly object _gate = new object();
		private readonly System.Collections.Generic.List<IEnvironmentTurnActor> _actors =
			new System.Collections.Generic.List<IEnvironmentTurnActor>();

		public void Add(IEnvironmentTurnActor actor)
		{
			if (actor == null) return;
			lock (_gate)
			{
				_actors.Add(actor);
			}
		}

		public void Remove(IEnvironmentTurnActor actor)
		{
			if (actor == null) return;
			lock (_gate)
			{
				_actors.Remove(actor);
			}
		}

		public System.Collections.Generic.IReadOnlyList<IEnvironmentTurnActor> Snapshot()
		{
			lock (_gate)
			{
				return _actors.ToArray();
			}
		}
	}
}

