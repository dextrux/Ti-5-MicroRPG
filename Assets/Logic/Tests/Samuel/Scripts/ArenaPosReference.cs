using UnityEngine;

public class ArenaPosReference : MonoBehaviour
{
    [SerializeField] private Transform _player;

    public Vector2 GetArenaRelativePosition(Transform objTransform)
    {
        return new Vector2(objTransform.position.x - transform.position.x, objTransform.position.z - transform.position.z);
    }

    public Vector2 GetPlayerArenaPosition()
    {
        return GetArenaRelativePosition(_player);
    }
}
