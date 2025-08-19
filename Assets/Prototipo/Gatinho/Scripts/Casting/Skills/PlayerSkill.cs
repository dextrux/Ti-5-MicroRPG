using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PlayerSkill : MonoBehaviour
{
    private List<Collider> _collidersInArea = new List<Collider>();
    private float _duration;

    public Action OnCast;

    private void Update()
    {
        if ((_duration -= Time.deltaTime) <= 0f)
        {
            Effect(_collidersInArea);
            OnCast?.Invoke();
        }
    }

    public void Setup(int duration)
    {
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
