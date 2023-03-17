using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance => _instance;

    /// <summary>
    /// Use this to check if SP!=null as the nullcheck is costly.
    /// </summary>
    public static bool Instance_exists = false;

    protected virtual void Awake()
    {
        if (_instance == null)
            setSingletonInstance();
    }

    /// <summary>
    /// Updates the `SP` static var to this instance. Usually done in Awake automatically, but if for some reason you dont want that use this method at the appropriate time.
    /// </summary>
    protected virtual void setSingletonInstance()
    {
        _instance = this as T;
        Instance_exists = _instance != null; //Do this for every SP call as in editor time objects can be gone without calling OnDestroy to clear this bool.
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        Instance_exists = _instance != null; //Do this for every SP call as in editor time objects can be gone without calling OnDestroy to clear this bool.
    }
}