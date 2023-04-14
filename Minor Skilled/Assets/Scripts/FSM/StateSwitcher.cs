using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    [Header("Tank states")]
    public E_TankState DefaultTankState = E_TankState.Combat;
    [SerializeField] private TankState _inspectionState;
    [SerializeField] private TankState _combatState;
    [SerializeField] private TankState _deathState;
    [SerializeField] private TankState _hostileInspectionState;

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
        if (_currentTankState != null)
        {
            if (_currentTankState.ThisState == newStateEnum)
                return;
            
            _currentTankState.ExitState();
        }

        TankState newState = newStateEnum switch
        {
            E_TankState.Inspection => _inspectionState,
            E_TankState.Combat => _combatState,
            E_TankState.Death => _deathState,
            E_TankState.HostileInspection => _hostileInspectionState,
        };

        newState.EnterState();
        _currentTankState = newState;
    }
}