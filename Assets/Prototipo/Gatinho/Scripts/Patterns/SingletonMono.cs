using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
            Destroy(this);
    }
}
