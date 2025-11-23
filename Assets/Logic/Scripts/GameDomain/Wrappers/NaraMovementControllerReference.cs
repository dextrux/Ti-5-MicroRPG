using System;
using UnityEngine;

[Serializable]
public class NaraMovementControllerReference {
    [SerializeField] private string _typeName;

    public Type ControllerType =>
        string.IsNullOrEmpty(_typeName) ? null : Type.GetType(_typeName);

    public void SetType(Type t) {
        _typeName = t.AssemblyQualifiedName;
    }
}
