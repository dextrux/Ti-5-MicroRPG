using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PlayerSkill : MonoBehaviour
{
    [SerializeField, Min(1)] private int _cost = 1;
    [SerializeField] private Renderer _visualArea;

    private List<Collider> _collidersInArea = new List<Collider>();
    private TurnoTatico _taticTurn;
    private int _duration;

    public Action OnCast;

    public int Cost { get { return _cost; } }
    public Renderer VisualArea { get { return _visualArea; } }

    public void DebitDuration()
    {
        _duration--;
    }

    public void TryCast()
    {
        if (_duration > 0) return;

        Effect(_collidersInArea);
        OnCast?.Invoke();

        _taticTurn.OnTurnBegin -= TryCast;
        _taticTurn.OnTurnEnd -= DebitDuration;
    }

    public void Setup(TurnoTatico taticTurn, int duration)
    {
        _taticTurn = taticTurn;
        _taticTurn.OnTurnBegin += TryCast;
        _taticTurn.OnTurnEnd += DebitDuration;

        _duration = duration;
    }

    protected abstract void Effect(List<Collider> colliders);

    private void OnTriggerEnter(Collider other)
    {
        _collidersInArea.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _collidersInArea.Remove(other);
    }
}
