using UnityEngine;

public class TankComponent : MonoBehaviour
{
    protected TankComponentManager componentManager;
    
    protected void Awake()
    {
        componentManager = GetComponent<TankComponentManager>();
    }
}