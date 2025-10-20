using UnityEngine;
using Logic.Scripts.Services.CommandFactory;
using Zenject;

namespace Logic.Scripts.GameDomain.Commands
{
    public struct SpawnOrbData
    {
        public ArenaPosReference Arena;
        public Vector3 Origin;
        public GameObject Prefab;
        public Logic.Scripts.GameDomain.MVC.Environment.Orb.OrbRegistry Registry;
        public float MoveStep;
        public float GrowStep;
        public float InitialRadius;
        public float MaxRadius;
        public int BaseDamage;
        public int InitialHp;
    }

    public class SpawnOrbCommand : BaseCommand, ICommandVoid
    {
        private SpawnOrbData _data;
        private DiContainer _container;
        private Logic.Scripts.GameDomain.MVC.Environment.Orb.OrbRegistry _registry;

        public SpawnOrbCommand SetData(SpawnOrbData data)
        {
            _data = data;
            return this;
        }

        public override void ResolveDependencies()
        {
            _container = _diContainer;
            // No-op: fallback path removed; we now rely on OrbController.Instances in the rule
        }

        public void Execute()
        {
            if (_data.Prefab == null) { Debug.LogWarning("SpawnOrbCommand: Prefab null"); return; }
            Debug.Log($"[SpawnOrb] Instantiating orb prefab at {_data.Origin}");
            GameObject orbGo = Object.Instantiate(_data.Prefab, _data.Origin, Quaternion.identity);
            if (!orbGo.activeSelf) orbGo.SetActive(true);
            var controller = orbGo.GetComponent<Logic.Scripts.GameDomain.MVC.Environment.Orb.OrbController>();
            if (controller != null)
            {
                controller.Initialize(_data.Arena, _registry, _data.MoveStep, _data.GrowStep, _data.InitialRadius, _data.MaxRadius, _data.BaseDamage, _data.InitialHp);
                // No registry registration; rule will use OrbController.Instances
            }
        }
    }
}


