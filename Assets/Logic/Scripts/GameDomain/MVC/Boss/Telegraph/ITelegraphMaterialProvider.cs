using System.Collections.Generic;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss.Telegraph
{
	public interface ITelegraphMaterialProvider
	{
		Material GetMaterial(bool telegraphDisplacementEnabled, IList<AbilityEffect> effects);
	}
}

