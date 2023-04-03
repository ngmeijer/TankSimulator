using UnityEngine;

public class TankComponent : MonoBehaviour
{
    protected TankComponentManager _componentManager;
    protected TankProperties _properties;
    
    protected virtual void Awake()
    {
        _componentManager = GetComponentInParent<TankComponentManager>();
        _properties = _componentManager.Properties;
    }
}