using UnityEngine;

public class PlayerInspectState : TankInspectState
{
    public override void Enter()
    {
        base.Enter();
        
        HandleDamageRegistrationUI(true);
    }

    public override void Exit()
    {
        base.Exit();
        
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