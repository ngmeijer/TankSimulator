using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TankParts
{
    Turret,
    LeftTrack,
    RightTrack,
    HullLeft,
    HullRight,
    HullFront,
    HullBack
}

public class DamageRegistrationComponent : TankComponent
{
    public int Health { get; private set; }
    public int Armor { get; private set; }
    
    public int MaxArmor { get; private set; }
    public int MaxHealth { get; private set; }

    private GameObject lastColliderHit;

    [SerializeField] private GameObject destroyedTankGFX;
    [SerializeField] private GameObject functioningTankGFX;
    [SerializeField] private Camera destroyedTankCamera;
    [SerializeField] private GameObject deathVFX;

    private void Start()
    {
        MaxHealth = componentManager.Properties.MaxHealth;
        MaxArmor = componentManager.Properties.MaxArmor;
        
        Health = MaxHealth;
        Armor = MaxArmor;
        
        componentManager.entityHUD.SetMaxHealth(MaxHealth);
        //componentManager.entityHUD.SetMaxArmor(MaxArmor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Shell")) return;
        if (!collision.collider.TryGetComponent(out Shell shell)) return;
        if (collision.collider.gameObject == lastColliderHit) return;
        lastColliderHit = collision.collider.gameObject;

        UpdateHealth(shell.Damage);
        
        componentManager.entityHUD.UpdateHealth(Health);

        string partHit = collision.GetContact(0).thisCollider.name;
        NotifyPopupForCollidedPart(partHit);
    }

    private void UpdateHealth(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            componentManager.HasDied = true;
            functioningTankGFX.SetActive(false);
            destroyedTankGFX.SetActive(true);
            if(destroyedTankCamera != null)
                destroyedTankCamera.gameObject.SetActive(true);
            deathVFX.SetActive(true);
            componentManager.eventManager.OnEntityDeath?.Invoke(componentManager);
        }
    }

    private void NotifyPopupForCollidedPart(string partName)
    {
        string formattedPartText = "";
        switch (partName)
        {
            case "HullFront":
                formattedPartText = "The front of the hull";
                break;
            case "HullBack":
                formattedPartText = "The back of the hull";
                break;
            case "HullLeft":
                formattedPartText = "The left side of the hull";
                break;
            case "HullRight":
                formattedPartText = "The right side of the hull";
                break;
            case "Turret":
                formattedPartText = "The turret";
                break;
            case "LeftTrack":
                formattedPartText = "The left tracks";
                break;
            case "RightTrack":
                formattedPartText = "The right tracks";
                break;
        }
        componentManager.eventManager.OnTankComponentHit.Invoke($"{formattedPartText} of {componentManager.Properties.TankName} has been hit!");
    }
}
