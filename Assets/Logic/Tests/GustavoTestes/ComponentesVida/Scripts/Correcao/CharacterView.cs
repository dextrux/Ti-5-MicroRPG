using UnityEngine;

public class CharacterView : MonoBehaviour
{
    public void UpdateStats(CharacterData data)
    {
        Debug.Log("HP: " + data.Health + " | AP: " + data.ActionPoints + " | Vivo: " + data.IsAlive);
    }
}
