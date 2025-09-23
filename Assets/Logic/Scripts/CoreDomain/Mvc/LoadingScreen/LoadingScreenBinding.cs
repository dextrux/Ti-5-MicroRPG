using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LoadingScreenBindingSO", menuName = "Scriptable Objects/UI Bindings/LoadingScreenBinding")]
public class LoadingScreenBinding : ScriptableObject
{
    public Length PercentLoaded;
}
