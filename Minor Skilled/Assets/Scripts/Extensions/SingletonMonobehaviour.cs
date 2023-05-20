using System;
using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;
    public static bool Instance_exists = false;

    protected virtual void Awake()
    {
        if (_instance == null)
            SetSingletonInstance();
        else throw new Exception($"Singleton instance of {_instance} already exists.");
    }

    protected void SetSingletonInstance()
    {
        _instance = this as T;
        Instance_exists = _instance != null; 
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        Instance_exists = _instance != null; 
    }
}