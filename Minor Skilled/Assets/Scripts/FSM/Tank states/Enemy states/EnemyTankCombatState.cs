using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTankCombatState : TankCombatState
{
    public bool IsReloading()
    {
        return _componentManager.ShootComponent.IsReloading;
    }
    
    public bool HasShells()
    {
        return _componentManager.ShootComponent.GetShellCount() > 0;
    }
    
    public void RotateTurret(float direction)
    {
        _componentManager.TurretControlComponent.HandleTurretRotation(direction, _componentManager.Properties.TP_HorizontalSensitivity);
    }

    public void FireShell()
    {
        _componentManager.ShootComponent.FireShell(); 
    }

    public float GetHealthPercentage()
    {
        float percent = _componentManager.DamageComponent.CurrentData.GetMostDamagedPartHealth();

        return percent;
    }
    
    public float GetArmorPercentage()
    {
        float percent = _componentManager.DamageComponent.CurrentData.GetMostDamagedPartArmor();

        return percent;
    }
}