using UnityEngine;

namespace Proto_Samuel
{
    public class PlayerEchoAction : MonoBehaviour
    {
        [SerializeField] private GameObject _echoPrefab;

        private void OnEnable()
        {
            InputObserver.Instance.OnSpaceDown += PlaceEcho;
        }

        private void OnDisable()
        {
            InputObserver.Instance.OnSpaceDown -= PlaceEcho;
        }

        private void PlaceEcho()
        {
            
        }
    }
}
