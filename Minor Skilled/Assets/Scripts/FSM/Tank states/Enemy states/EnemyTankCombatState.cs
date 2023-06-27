using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTankCombatState : TankCombatState
{
    public bool IsReloading()
    {
        return _componentManager.ShootComp.IsReloading;
    }
    
    public bool HasShells()
    {
        return _componentManager.ShootComp.GetShellCount() > 0;
    }
    
    public void RotateTurret(float direction)
    {
        _componentManager.TurretControlComp.HandleTurretRotation(direction, _componentManager.Properties.TP_HorizontalSensitivity);
    }

    public void FireShell()
    {
        _componentManager.ShootComp.FireShell(); 
    }

    public float GetHealthPercentage()
    {
        float percent = _componentManager.DamageComp.CurrentData.GetMostDamagedPartHealth();

        return percent;
    }
    
    public float GetArmorPercentage()
    {
        float percent = _componentManager.DamageComp.CurrentData.GetMostDamagedPartArmor();

        return percent;
    }

    public void RepairTank()
    {
        _componentManager.DamageComp.RepairAllParts();
    }
}