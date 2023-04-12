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
    
    private int _currentCameraFOVLevel;
    public int CurrentFOV
    {
        get { return _currentCameraFOVLevel; }
    }

    private PlayerInputActions _inputActions;

    private void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Tankmovement.ZoomADS.started += ZoomADS;
        _inputActions.Enable();
    }
    
    public override void EnterState()
    {
        base.EnterState();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        HUDManager.Instance.SetZoomLevelText(_currentCameraFOVLevel + 1, true);
    }

    public override int GetFOVLevel()
    {
        return CurrentFOV;
    }
    
    public void ZoomADS(InputAction.CallbackContext cb)
    {
        if (_currentCameraFOVLevel >= _fovRanges.Length - 1)
            _currentCameraFOVLevel = 0;
        else _currentCameraFOVLevel++;

        float newFOV = _fovRanges[_currentCameraFOVLevel];
        ViewCam.fieldOfView = newFOV;

        HUDManager.Instance.SetZoomLevelText(_currentCameraFOVLevel + 1, true);
    }

    public override void ExitState()
    {
        base.ExitState();
        
        HUDManager.Instance.SetZoomLevelText(_currentCameraFOVLevel + 1, false);
    }
}