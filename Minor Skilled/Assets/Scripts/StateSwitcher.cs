using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    [Header("Tank states")]
    public E_TankState DefaultTankState = E_TankState.Combat;
    [SerializeField] private TankState _inspectionState;
    [SerializeField] private TankState _combatState;
    [SerializeField] private TankState _deathState;
    [SerializeField] private TankState _hostileInspectionState;
    
    public TankState CurrentTankState
    {
        get => _currentTankState;
        set => _currentTankState = value;
    }
    private TankState _currentTankState;
    
    protected virtual void Update()
    {
        if (_currentTankState == null) return;
        _currentTankState.UpdateState();
    }

    protected virtual void FixedUpdate()
    {
        if (_currentTankState == null) return;
        _currentTankState.FixedUpdateState();
    }

    protected virtual void LateUpdate()
    {
        if (_currentTankState == null) return;
        _currentTankState.LateUpdateState();
    }
    
    public void SwitchToTankState(E_TankState newStateEnum)
    {
        if(_currentTankState != null)
            _currentTankState.ExitState();

        TankState newState = null;
        
        switch (newStateEnum)
        {
            case E_TankState.Inspection:
                newState = _inspectionState;
                break;
            case E_TankState.Combat:
                newState = _combatState;
                break;
            case E_TankState.Death:
                newState = _deathState;
                break;
            case E_TankState.HostileInspection:
                newState = _hostileInspectionState;
                break;
        }
        
        newState.EnterState();
        _currentTankState = newState;
    }
}