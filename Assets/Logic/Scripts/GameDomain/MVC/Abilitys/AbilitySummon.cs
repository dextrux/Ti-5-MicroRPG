using Logic.Scripts.GameDomain.MVC.Environment.Orb;
using Logic.Scripts.Turns;
using System.Threading.Tasks;
using UnityEngine;

public class AbilitySummon : MonoBehaviour, IEnvironmentTurnActor {
    public bool RemoveAfterRun => NeedRemoveAfter();

    [field: SerializeField] public GameObject VisualRoot { get; private set; }
    [SerializeField] private float _radius;
    [SerializeField] private Color _healAreaColor;
    private int _duration;
    private int _healAmount;
    private IEffectable _caster;
    private OrbView _areaView;

    public void SetUp(int duration, int healAmount, IEffectable caster) {
        _duration = duration;
        _healAmount = healAmount;
        _caster = caster;
        _areaView = GetComponent<OrbView>();
        _areaView.PrepareTelegraph();
        _areaView.UpdateColor(_healAreaColor);
        _areaView.UpdateRadius(_radius);
    }

    public Task ExecuteAsync() {
        if (_caster != null) {
            if (Vector3.Distance(transform.position, _caster.GetReferenceTransform().position) <= _radius) {
                _caster.Heal(_healAmount);
            }
        }
        _duration--;
        return Task.CompletedTask;
    }

    private bool NeedRemoveAfter() {
        if (_duration < 0) {
            Destroy(gameObject);
            return true;
        }
        else {
            return false;
        }
    }
    private void OnDestroy() {
        if (Logic.Scripts.Turns.EnvironmentActorsRegistryService.Instance != null) {
            Logic.Scripts.Turns.EnvironmentActorsRegistryService.Instance.Remove(this);
        }
    }
}
