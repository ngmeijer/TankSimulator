using System.Collections.Generic;
using Tank_components;
using TankComponents;
using UnityEngine;

namespace TankComponents
{
    [ExecuteAlways]
    public class TankComponentManager : MonoBehaviour
    {
        public StateSwitcher ThisStateSwitcher;
        public TankProperties Properties;
        public EventManager EventManager { get; private set; }
        public NewMoveComponent MoveComp { get; private set; }
        public TurretControlComponent TurretControlComp { get; private set; }
        public ShootComponent ShootComp { get; private set; }
        public DamageRegistrationComponent DamageComp { get; private set; }

        public Transform EntityOrigin;
        public Transform Raycaster;
    
        public bool HasDied;
        public int ID;
    
        private void Awake()
        {
            ThisStateSwitcher = GetComponent<StateSwitcher>();
            Debug.Assert(ThisStateSwitcher != null, $"StateSwitcher is not attached to {transform.name}'s component holder.");
        
            MoveComp = GetComponentInChildren<NewMoveComponent>();
            Debug.Assert(MoveComp != null, $"NewMoveComponent is not attached to {transform.name}'s component holder.");
        
            TurretControlComp = GetComponentInChildren<TurretControlComponent>();
            Debug.Assert(TurretControlComp != null, $"TurretControlComponent is not attached to {transform.name}'s component holder.");

            ShootComp = GetComponentInChildren<ShootComponent>();
            Debug.Assert(TurretControlComp != null, $"ShootComponent is not attached to {transform.name}'s component holder.");

            DamageComp = GetComponentInChildren<DamageRegistrationComponent>();
            Debug.Assert(TurretControlComp != null, $"DamageRegistrationComponent is not attached to {transform.name}'s component holder.");

            EventManager = GetComponent<EventManager>();
            Debug.Assert(TurretControlComp != null, $"EventManager is not attached to {transform.name}.");
        }

        private void Start()
        {
            EventManager.OnTankDestruction.AddListener(OnTankDeath);
        }

        private void OnTankDeath(int entityID)
        {
            HasDied = true;
        }

        public Vector3 GetCurrentBarrelDirection() => TurretControlComp.GetCurrentBarrelDirection();
    
        public List<WheelCollider> GetLeftWheelColliders()
        {
            // if (MoveComponent != null)
            //     return MoveComponent.GetLeftWheelColliders();
        
            Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
            return null;
        }

        public List<WheelCollider> GetRightWheelColliders()
        {
            // if (MoveComponent != null)
            //     return MoveComponent.GetRightWheelColliders();
        
            Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
            return null;
        }
    }
}