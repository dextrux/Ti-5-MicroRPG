using UnityEngine;
using Zenject;
using Logic.Scripts.GameDomain.MVC.Nara;

public class ArenaPosReference : MonoBehaviour
{
    private INaraController _naraController;

    [Inject]
    public void Construct(INaraController naraController)
    {
        _naraController = naraController;
    }

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
        return RealPositionToRelativeArenaPosition(_naraController.NaraViewGO.transform);
    }
}
