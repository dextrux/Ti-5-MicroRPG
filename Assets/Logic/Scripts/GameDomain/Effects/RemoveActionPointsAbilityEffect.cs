using System;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.Effects
{
	[Serializable]
	public sealed class RemoveActionPointsAbilityEffect : AbilityEffect
	{
		public int amount = 1;

		public override void Execute(IEffectable caster, IEffectable target)
		{
			if (target is IEffectableAction act)
			{
				act.SubtractActionPoints(amount);
			}
			// if not player (no IEffectableAction), ignore
		}
	}
}


