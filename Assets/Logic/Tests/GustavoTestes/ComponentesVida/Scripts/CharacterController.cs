using Zenject;

public class CharacterController : ITickable
{
    private readonly CharacterModel model;
    private readonly CharacterView view;

    [Inject]
    public CharacterController(CharacterModel model, CharacterView view)
    {
        this.model = model;
        this.view = view;

        model.OnHealthChanged += view.UpdateHealth;
        model.OnActionPointsChanged += view.UpdateActionPoints;
        model.OnStatesChanged += view.UpdateStates;
        model.OnInvulnerabilityChanged += view.UpdateInvulnerability;
    }

    public void Tick()
    {
        //Efeitos que duram mais de um turno, veneno, burn, ...
    }

    public void ApplyState(IStatusEffect state)
    {
        state.Apply(model);
        model.AddState(state);
    }

    public void Damage(int value) => model.TakeDamage(value);
    public void Heal(int value) => model.Heal(value);
    public void SetInvulnerability(bool value) => model.SetInvulnerable(value);
}
