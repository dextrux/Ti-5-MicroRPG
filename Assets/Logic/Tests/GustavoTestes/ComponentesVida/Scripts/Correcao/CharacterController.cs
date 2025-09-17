public class CharacterController
{
    private CharacterData data;
    private CharacterView view;

    public CharacterController(CharacterData data, CharacterView view)
    {
        this.data = data;
        this.view = view;
    }

    public void ModifyHealth(int value)
    {
        data.Health += value;
        if (data.Health <= 0)
        {
            data.Health = 0;
            data.IsAlive = false;
        }
        
    }

    public void ModifyActionPoints(int value)
    {
        data.ActionPoints += value;
        if (data.ActionPoints < 0)
        {
            data.ActionPoints = 0;
        }
        
    }

    public void EndTurn()
    {
        //Logica de fim de turno
    }
}
