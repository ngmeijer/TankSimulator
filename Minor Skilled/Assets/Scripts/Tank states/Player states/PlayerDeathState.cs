using UnityEngine;

public class PlayerDeathState : TankDeathState
{
    protected override void OnDeathActions()
    {
        base.OnDeathActions();
        
        HUDManager.Instance.EnableCombatUI(false);
        HUDManager.Instance.EnableMenuUI(true);
    }
}