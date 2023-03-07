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

    private void Start()
    {
        MaxHealth = componentManager.Properties.MaxHealth;
        MaxArmor = componentManager.Properties.MaxArmor;
        
        Health = MaxHealth;
        Armor = MaxArmor;
        
        componentManager.HUDManager.SetMaxHealth(MaxHealth);
        componentManager.HUDManager.SetMaxArmor(MaxArmor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Shell")) return;
        if (!collision.collider.TryGetComponent(out Shell shell))
        {
            Debug.Log("failed to get Shell component");
            return;
        }
        
        int totalArmorLost = CalculateArmorLeft(shell);
        CalculateHealthLeft(shell, totalArmorLost);
        
        componentManager.HUDManager.UpdateHealth(Health);
        componentManager.HUDManager.UpdateArmor(Armor);

        string partHit = collision.GetContact(0).thisCollider.name;
        string formattedPartText = "";
        switch (partHit)
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
        componentManager.eventManager.OnTankComponentHit.Invoke($"{formattedPartText} has been hit!");
    }

    private int CalculateArmorLeft(Shell shell)
    {
        int relativeArmorPenetrated = shell.RelativeArmorPenetration * Health;
        int absoluteArmorPenetrated = shell.AbsoluteArmorPenetration * Health;
        int totalArmorLost = relativeArmorPenetrated + absoluteArmorPenetrated;
        Armor -= totalArmorLost;
        return totalArmorLost;
    }

    private void CalculateHealthLeft(Shell shell, int totalArmorLost)
    {
        int damageReceived = shell.Damage - totalArmorLost;
        if (damageReceived > 0)
            Health -= damageReceived;
    }
}
