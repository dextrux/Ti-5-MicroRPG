using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public interface IEchoController {
        public void CreateFastEcho(Transform referenceTransform);

        public void CreateSlowEcho(Transform referenceTransform);
    }
}