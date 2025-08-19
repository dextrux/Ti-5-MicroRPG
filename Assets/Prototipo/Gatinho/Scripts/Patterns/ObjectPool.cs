using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _instanceObject;   
    private List<GameObject> _objects = new List<GameObject>();

    // Get & Set
    public void SetInstanceObject(GameObject obj)
    {
        _instanceObject = obj;
    }

    // Public Methods

    #region // CreateObjectPool

    public static ObjectPool CreateObjecPool(Vector3 poolPosition)
    {
        return m_CreateObjecPool("ObjectPool", poolPosition, null);
    }

    public static ObjectPool CreateObjecPool(string name, Vector3 poolPosition)
    {
        return m_CreateObjecPool(name, poolPosition, null);
    }

    public static ObjectPool CreateObjecPool(Vector3 poolPosition, Transform poolParent)
    {
        return m_CreateObjecPool("ObjectPool", poolPosition, poolParent);
    }

    public static ObjectPool CreateObjecPool(string name, Vector3 poolPosition, Transform poolParent)
    {
        return m_CreateObjecPool(name, poolPosition, poolParent);
    }

    // Private
    private static ObjectPool m_CreateObjecPool(string name, Vector3 poolPosition, Transform poolParent)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = poolPosition;
        obj.transform.parent = poolParent;
        return obj.AddComponent<ObjectPool>();
    }

    #endregion

    #region // InstantiateObject

    public GameObject InstantiateObject(Vector3 instancePosition)
    {
        return m_InstantiateObject(instancePosition, Quaternion.identity, null);
    }

    public GameObject InstantiateObject(Vector3 instancePosition, Quaternion instanceRotation)
    {
        return m_InstantiateObject(instancePosition, instanceRotation, null);
    }

    public GameObject InstantiateObject(Vector3 instancePosition, Transform instaceParent)
    {
        return m_InstantiateObject(instancePosition, Quaternion.identity, instaceParent);
    }

    public GameObject InstantiateObject(Vector3 instancePosition, Quaternion instanceRotation, Transform instaceParent)
    {
        return m_InstantiateObject(instancePosition, instanceRotation, instaceParent);
    }

    // Private
    private GameObject m_InstantiateObject(Vector3 instancePosition, Quaternion instanceRotation, Transform instaceParent)
    {
        if (_instanceObject == null)
        {
            Debug.LogError("No instance object assigned to the pool!");
            return null;
        }

        GameObject obj;

        if (_objects.Count <= 0)
        {
            obj = Instantiate(_instanceObject, transform.position, instanceRotation, instaceParent);
        }
        else
        {
            obj = _objects[0];
            _objects.Remove(obj);
        }

        obj.transform.position = instancePosition;
        obj.SetActive(true);

        return obj;
    }

    #endregion

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = transform.position;
        _objects.Add(obj);
    }
}
