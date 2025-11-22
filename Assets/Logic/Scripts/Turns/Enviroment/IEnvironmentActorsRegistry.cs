namespace Logic.Scripts.Turns
{
	public interface IEnvironmentActorsRegistry
	{
		void Add(IEnvironmentTurnActor actor);
		void Remove(IEnvironmentTurnActor actor);
		System.Collections.Generic.IReadOnlyList<IEnvironmentTurnActor> Snapshot();
	}
}
