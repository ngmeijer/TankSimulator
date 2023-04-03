using System;
using UnityEngine;

public class PlayerInput : TankComponent
{
    private float _moveInput;
    private float _hullRotateInput;
    private float _scrollInput;
    private float _mouseHorizontalInput;
    private float _cannonTiltInput;

    [SerializeField] private InspectState _inspectState;
    [SerializeField] private CombatState _combatState;
    private TankState _currentState;

    protected override void Awake()
    {
        base.Awake();
        
        _inspectState = GetComponentInChildren<InspectState>();
        _combatState = GetComponentInChildren<CombatState>();
    }

    private void Start()
    {
        HUDManager.Instance.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
        HUDManager.Instance.UpdateShellTypeUI(_componentManager.ShootComponent.GetCurrentShellType());

        SwitchToState(_combatState);
    }

    private void Update()
    {
        if (_componentManager.HasDied) return;
        CheckStateSwitch();
        
        _currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    private void LateUpdate()
    {
        _currentState.LateUpdateState();
    }

    private void CheckStateSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _combatState.CameraComponent.EnableADS();
            SwitchToState(_combatState);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToState(_combatState);
            _combatState.CameraComponent.EnableThirdPerson();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToState(_inspectState);
            _combatState.CameraComponent.EnableInspectCamera();
        }
    }

    private void SwitchToState(TankState newState)
    {
        if (newState == _currentState)
        {
            Debug.Log($"NewState: {newState}. CurrentState: {_currentState}");
            return;
        }
        
        _currentState?.ExitState();
        _currentState = newState;
        _currentState?.EnterState();
    }
}
