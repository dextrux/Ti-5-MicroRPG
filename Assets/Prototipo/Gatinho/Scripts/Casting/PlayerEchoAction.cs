using UnityEngine;

public class PlayerEchoAction : MonoBehaviour
{
    [SerializeField] private GameObject _echoPrefab;

    [Header("ObjectPool")]
    [SerializeField] private Vector3 _poolAbsolutePosition;
    private ObjectPool _objectPool;   

    private void Awake()
    {
        _objectPool = ObjectPool.CreateObjecPool("EchoInstancePool", _poolAbsolutePosition);
        _objectPool.SetInstanceObject(_echoPrefab);
    }

    public void PlaceEcho(PlayerSkill echoSkill)
    {
        if (!_echoPrefab.TryGetComponent(out EchoInstance auxGetEchoInstance))
        {
            Debug.LogWarning("'EchoPrefab' must have 'EchoInstance'");
            return;
        }

        EchoInstance echoInstance = _objectPool.InstantiateObject(transform.position, transform.rotation, _objectPool.transform).GetComponent<EchoInstance>();
        echoInstance.CastSetup(echoSkill, _objectPool);
    }
}
