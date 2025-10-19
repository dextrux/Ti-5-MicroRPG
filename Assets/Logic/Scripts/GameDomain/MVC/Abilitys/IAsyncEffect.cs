namespace Logic.Scripts.GameDomain.MVC.Abilitys
{
    public interface IAsyncEffect
    {
        System.Collections.IEnumerator ExecuteRoutine(IEffectable caster, IEffectable target);
    }
}


