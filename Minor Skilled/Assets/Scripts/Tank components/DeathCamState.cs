public class DeathCamState : CameraState
{
    public override void EnterState()
    {
        base.EnterState();
        
        OnDeathActions();
    }

    private void OnDeathActions()
    {
        
    }
}