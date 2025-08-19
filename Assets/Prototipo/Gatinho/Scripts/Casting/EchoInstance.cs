using UnityEngine;

public class EchoInstance : MonoBehaviour
{
    private ObjectPool _returnPool;

    public void CastSetup(PlayerSkill skill, ObjectPool returnPool)
    {
        skill.OnCast += Disperse;

        _returnPool = returnPool;
    }

    private void Disperse()
    {
        _returnPool.ReturnObject(this.gameObject);
        _returnPool = null;
    }
}
