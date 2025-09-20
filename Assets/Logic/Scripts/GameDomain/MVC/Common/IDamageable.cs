namespace Logic.Scripts.GameDomain.MVC.Common
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        void Heal(int amount);
        void AddShield(int amount);
        bool IsAlive();
    }
}
