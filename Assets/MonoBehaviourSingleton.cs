using System;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // ReSharper disable once StaticMemberInGenericType
    private static T _instance;
    public static T Instance => _instance;
    
    public virtual void Awake()
    {
        Assert.IsNull(_instance, "There can only be one instance of " + typeof(T).FullName);
        _instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}