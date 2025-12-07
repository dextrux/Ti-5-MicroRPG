using System.Collections.Generic;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss.Telegraph
{
	public class TelegraphMaterialProvider : ITelegraphMaterialProvider
	{
		private readonly TelegraphMaterialConfig _config;

		public TelegraphMaterialProvider(TelegraphMaterialConfig config)
		{
			_config = config;
		}

		public Material GetMaterial(bool telegraphDisplacementEnabled, IList<AbilityEffect> effects)
		{
			UnityEngine.Debug.Log($"[TelegraphProvider] GetMaterial called. telegraphDisplacementEnabled={telegraphDisplacementEnabled} cfg={( _config != null ? _config.name : "NULL")}");
			if (_config == null)
			{
				UnityEngine.Debug.LogWarning("[TelegraphProvider] Config is NULL. Falling back to Sprites/Default.");
				return new Material(Shader.Find("Sprites/Default"));
			}

			if (!telegraphDisplacementEnabled)
			{
				var mat = _config.NormalAreaMaterial != null ? _config.NormalAreaMaterial : new Material(Shader.Find("Sprites/Default"));
				UnityEngine.Debug.Log($"[TelegraphProvider] Using Normal material: {(mat != null ? mat.name : "NULL")}");
				return mat;
			}

			bool hasGrapple = false;
			bool hasKnock = false;
			if (effects != null)
			{
				for (int i = 0; i < effects.Count; i++)
				{
					var fx = effects[i];
					if (fx == null) continue;
					if (fx is Logic.Scripts.GameDomain.Effects.GrappleEffect) hasGrapple = true;
					else if (fx is Logic.Scripts.GameDomain.Effects.KnockbackEffect) hasKnock = true;
				}
			}

			if (hasGrapple && _config.GrappleAreaMaterial != null) {
				UnityEngine.Debug.Log($"[TelegraphProvider] Using Grapple material: {_config.GrappleAreaMaterial.name}");
				return _config.GrappleAreaMaterial;
			}
			if (hasKnock && _config.KnockbackAreaMaterial != null) {
				UnityEngine.Debug.Log($"[TelegraphProvider] Using Knockback material: {_config.KnockbackAreaMaterial.name}");
				return _config.KnockbackAreaMaterial;
			}
			var fallback = _config.NormalAreaMaterial != null ? _config.NormalAreaMaterial : new Material(Shader.Find("Sprites/Default"));
			UnityEngine.Debug.Log($"[TelegraphProvider] Using Fallback Normal material: {(fallback != null ? fallback.name : "NULL")}");
			return fallback;
		}
	}
}

