using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;

public class SummonSkillCommand : BaseCommand, ICommandVoid {

    private IEnvironmentActorsRegistry _environmentActorsRegistry;

    private SummonSkillCommandData _summonData;

    public SummonSkillCommand SetData(SummonSkillCommandData summonData) {
        _summonData = summonData;
        return this;
    }

    public override void ResolveDependencies() {
        _environmentActorsRegistry = _diContainer.Resolve<IEnvironmentActorsRegistry>();
    }

    public void Execute() {
        _environmentActorsRegistry.Add(_summonData.SummonedObject);
    }
}
