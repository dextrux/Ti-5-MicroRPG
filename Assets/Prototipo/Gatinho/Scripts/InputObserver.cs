using System;
using UnityEngine;

public class InputObserver : SingletonMono<InputObserver>
{
    #region // Actions

    public Action OnNum1Down;
    public Action OnNum2Down;
    public Action OnNum3Down;

    public Action OnCDown;
    public Action OnEDown;
    public Action OnQDown;
    public Action OnVDown;

    public Action OnSpaceDown;

    public Action OnMouse0Down;
    public Action OnMouse1Down;

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnNum1Down?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            OnNum2Down?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            //OnNum3Down?.Invoke();

        if (Input.GetKeyDown(KeyCode.C))
            OnCDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.E))
            OnEDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.Q))
            OnQDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.V))
            OnVDown?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
            OnSpaceDown?.Invoke();

        if (Input.GetMouseButtonDown(0))
            OnMouse0Down?.Invoke();


        if (Input.GetMouseButtonDown(1))
            OnMouse1Down?.Invoke();
    }
}
