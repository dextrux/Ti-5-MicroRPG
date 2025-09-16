public class CharacterData
{
    public int Health;
    public int ActionPoints;
    public bool IsAlive;

    public CharacterData(int health, int actionPoints)
    {
        Health = health;
        ActionPoints = actionPoints;
        IsAlive = true;
    }
}
