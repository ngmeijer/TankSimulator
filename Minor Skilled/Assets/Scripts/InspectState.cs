using UnityEngine;

public class InspectState : TankState
{
    private float _scrollInput;
    private float _mouseHorizontalInput;

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
        CameraComponent.RotateAroundTank(_mouseHorizontalInput, _scrollInput);
    }

    protected override void GetInputValues()
    {
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        _mouseHorizontalInput = Input.GetAxis("Mouse X");
    }
    
    private void HandleDamageRegistrationUI(bool enabled)
    {
        HUDManager.Instance.EnableDamageUI(enabled, ComponentManager.DamageComponent.CurrentData);
    }
}