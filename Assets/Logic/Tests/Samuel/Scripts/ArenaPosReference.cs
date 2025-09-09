using UnityEngine;

public class ArenaPosReference : MonoBehaviour
{
    [SerializeField] private Transform _player;

    public Vector2 RealPositionToRelativeArenaPosition(Transform objTransform)
    {
        return new Vector2(objTransform.position.x - transform.position.x, objTransform.position.z - transform.position.z);
    }

    public Vector3 RelativeArenaPositionToRealPosition(Vector2 coords)
    {
        return new Vector3(coords.x + transform.position.x, transform.position.y, coords.y + transform.position.z);
    }

    public Vector2 GetPlayerArenaPosition()
    {
        return RealPositionToRelativeArenaPosition(_player);
    }
}
