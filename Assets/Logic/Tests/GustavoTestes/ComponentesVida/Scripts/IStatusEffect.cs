public interface IStatusEffect
{
    string Name { get; }
    void Apply(CharacterModel character);
    void Remove(CharacterModel character);
}
