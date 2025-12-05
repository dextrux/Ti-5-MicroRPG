using System;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.Effects
{
	[Serializable]
	public sealed class HealAbilityEffect : AbilityEffect
	{
		public int amount = 5;

		public override void Execute(IEffectable caster, IEffectable target)
		{
			if (target == null) return;
			target.Heal(amount);
		}
	}
}


