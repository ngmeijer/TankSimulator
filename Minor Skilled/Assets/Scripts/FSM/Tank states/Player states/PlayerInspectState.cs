using UnityEngine;

public class PlayerInspectState : TankInspectState
{
    public override void EnterState()
    {
        base.EnterState();
        
        HandleDamageRegistrationUI(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        
        HandleDamageRegistrationUI(false);
    }

    public override void UpdateState()
    {
        GetInputValues();
    }

    private void HandleDamageRegistrationUI(bool enabled)
    {
        _componentManager.DamageComponent.ShowUI(enabled);
    }
}