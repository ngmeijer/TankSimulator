using UnityEngine;

public class TankComponent : MonoBehaviour
{
    protected TankComponentManager componentManager;
    
    protected virtual void Awake()
    {
        componentManager = GetComponent<TankComponentManager>();
    }
}