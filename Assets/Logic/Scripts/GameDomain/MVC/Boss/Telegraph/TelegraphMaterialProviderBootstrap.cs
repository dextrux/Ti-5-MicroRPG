using Zenject;

namespace Logic.Scripts.GameDomain.MVC.Boss.Telegraph
{
	// Publica serviços globais após os installers concluírem.
	public sealed class TelegraphMaterialProviderBootstrap : IInitializable
	{
		private readonly ITelegraphMaterialProvider _provider;
		private readonly ITelegraphLayeringService _layering;

		public TelegraphMaterialProviderBootstrap(
			ITelegraphMaterialProvider provider,
			ITelegraphLayeringService layering)
		{
			_provider = provider;
			_layering = layering;
		}

		public void Initialize()
		{
			TelegraphMaterialService.Provider = _provider;
			TelegraphLayeringLocator.Service = _layering;
			UnityEngine.Debug.Log("[TelegraphProvider] Published provider and layering service");
		}
	}
}