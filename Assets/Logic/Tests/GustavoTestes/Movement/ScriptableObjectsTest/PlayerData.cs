using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float velocity = 5f;
    public float rotation = 10f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float gravity = 10f;
}
