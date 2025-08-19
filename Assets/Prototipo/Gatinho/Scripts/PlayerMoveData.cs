using UnityEngine;

namespace Proto_Samuel
{
    [CreateAssetMenu(fileName = "Player Nove Data", menuName = "Scriptable Objects/Data/PlayerMoveData")]
    public class PlayerMoveData : ScriptableObject
    {
        [Header("Move")]
        [SerializeField] public float moveSpeed = 5f;
        [SerializeField] public float rotateSpeed = 2f;
        [SerializeField] public float turnSmoothTime = 0.1f;
    }
}
