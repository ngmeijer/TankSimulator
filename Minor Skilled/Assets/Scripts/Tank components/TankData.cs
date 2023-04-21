using System;
using System.Collections.Generic;

[Serializable]
public class TankData
{
    //Health cannot be regained! (Besides some kind of upgrade/powerup etc?)
    //Armor will first be degraded. Once armor is depleted, damage starts affecting the health
    //If any of the components get destroyed, you die.
    
    public List<TankPart> TankParts = new List<TankPart>();

    public int GetMaxTotalHealth()
    {
        int totalHealth = 0;
        foreach (var part in TankParts)
        {
            totalHealth += part.MaxHealth;
        }

        return totalHealth;
    }

    public int GetMaxTotalArmor()
    {
        int totalArmor = 0;
        foreach (var part in TankParts)
        {
            totalArmor += part.MaxArmor;
        }

        return totalArmor;
    }
    
    public int GetCurrentTotalHealth()
    {
        int currentHealth = 0;
        foreach (var part in TankParts)
        {
            currentHealth += part.CurrentHealth;
        }

        return currentHealth;
    }
    
    public int GetCurrentTotalArmor()
    {
        int currentArmor = 0;
        foreach (var part in TankParts)
        {
            currentArmor += (int)part.CurrentArmor;
        }

        return currentArmor;
    }
}