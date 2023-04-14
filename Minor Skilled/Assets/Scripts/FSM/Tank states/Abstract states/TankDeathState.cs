
using UnityEngine;

public abstract class TankDeathState : TankState
{
    [SerializeField] private GameObject _functioningTankGFX;
    [SerializeField] private GameObject _destroyedTankGFX;
    [SerializeField] private GameObject _deathVFX;
    
    public override void EnterState()
    {
        base.EnterState();

        OnDeathActions();
    }
    
    protected virtual void OnDeathActions()
    {
        _componentManager.EventManager.OnTankDestruction.Invoke(_componentManager.ID);
        _functioningTankGFX.SetActive(false);
        _destroyedTankGFX.SetActive(true);
        _deathVFX.SetActive(true);
        _componentManager.HasDied = true;
    }
}