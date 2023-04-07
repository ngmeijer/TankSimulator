public class DeathCamState : CameraState
{
    public override void EnterState()
    {
        base.EnterState();
        
        OnDeathActions();
    }
    
    public override void UpdateState()
    {
        
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
    
    private void OnDeathActions()
    {
        
    }
}