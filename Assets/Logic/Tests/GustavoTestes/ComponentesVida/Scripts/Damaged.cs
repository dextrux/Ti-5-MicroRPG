public class Damaged : IStatusEffect
{
    private int damage;
    public string Name => $"Damaged({damage})";

    public Damaged(int damage)
    {
        this.damage = damage;
    }

    public void Apply(CharacterModel character)
    {
        character.TakeDamage(damage);
    }

    public void Remove(CharacterModel character)
    {
        // Dano é instantaneo, nao ha nada para remover
    }
}
