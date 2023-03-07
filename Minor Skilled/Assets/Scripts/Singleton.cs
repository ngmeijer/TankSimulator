using UnityEngine;

public class Singleton : MonoBehaviour
{
    protected static Singleton _instance;

    public static Singleton Instance { get { return _instance; } }

    protected void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this);
        else _instance = this;
    }
}