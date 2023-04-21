using UnityEngine;
using UnityEngine.InputSystem;

public class AdsState : CameraState
{
    private float[] _fovRanges = {
        60f,
        20f,
        10f,
        5f,
        2.5f
    };
    private int _currentFOVIndex;

    private PlayerInputActions _inputActions;

    private HUDCombatState _hudCombatState;

    private void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.TankMovement.ZoomADS.started += ZoomADS;
        _inputActions.Enable();
        
        _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
    }
    
    public override void EnterState()
    {
        base.EnterState();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _currentFOVIndex = 0;
        ViewCam.fieldOfView = _fovRanges[_currentFOVIndex];
        _hudCombatState.SetZoomLevelText(_currentFOVIndex + 1, true);
    }

    public override int GetFOVLevel()
    {
        return _currentFOVIndex;
    }
    
    private void ZoomADS(InputAction.CallbackContext cb)
    {
        if (_currentFOVIndex >= _fovRanges.Length - 1)
            _currentFOVIndex = 0;
        else _currentFOVIndex++;
        
        float newFOV = _fovRanges[_currentFOVIndex];
        ViewCam.fieldOfView = newFOV;

        _hudCombatState.SetZoomLevelText(_currentFOVIndex + 1, true);
    }

    public override void ExitState()
    {
        base.ExitState();
        
        _hudCombatState.SetZoomLevelText(_currentFOVIndex + 1, false);
    }
}