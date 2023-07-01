using UnityEngine;

namespace TankComponents
{
    public class TankComponent : MonoBehaviour
    {
        protected TankComponentManager _componentManager;
        protected TankProperties _properties;
    
        protected virtual void Awake()
        {
            _componentManager = GetComponentInParent<TankComponentManager>();   
            Debug.Assert(_componentManager != null, $"TankComponentManager on {transform.parent.name} is null.");
            _properties = _componentManager.Properties;
        }
    }
}