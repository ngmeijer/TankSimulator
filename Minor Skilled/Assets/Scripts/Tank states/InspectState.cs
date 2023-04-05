using UnityEngine;

public class InspectState : TankState
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

    public override void FixedUpdateState()
    {
        
    }

    public override void LateUpdateState()
    {
        
    }

    protected override void GetInputValues()
    {
        
    }
    
    private void HandleDamageRegistrationUI(bool enabled)
    {
        ComponentManager.DamageComponent.ShowUI(enabled);
    }
}