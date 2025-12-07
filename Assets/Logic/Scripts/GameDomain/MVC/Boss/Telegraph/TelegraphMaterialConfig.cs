using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss.Telegraph
{
	[CreateAssetMenu(fileName = "TelegraphMaterialConfig", menuName = "Scriptable Objects/Telegraph Material Config")]
	public class TelegraphMaterialConfig : ScriptableObject
	{
		public Material NormalAreaMaterial;
		public Material GrappleAreaMaterial;
		public Material KnockbackAreaMaterial;
	}
}

