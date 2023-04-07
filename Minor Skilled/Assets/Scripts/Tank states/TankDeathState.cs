using UnityEngine;

public class TankDeathState : TankState
{
    [SerializeField] private GameObject _functioningTankGFX;
    [SerializeField] private GameObject _destroyedTankGFX;
    [SerializeField] private GameObject _deathVFX;
    
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
        _functioningTankGFX.SetActive(false);
        _destroyedTankGFX.SetActive(true);
        _deathVFX.SetActive(true);
    }
}