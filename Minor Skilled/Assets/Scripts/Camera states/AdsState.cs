using UnityEngine;

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

    private void Start()
    {
        HUDManager.Instance.SetZoomLevelText(_currentCameraFOVLevel + 1);
    }
    
    public override void EnterState()
    {
        base.EnterState();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void UpdateState()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        
        ZoomADS();
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
    
    public void ZoomADS()
    {
        if (_currentCameraFOVLevel >= _fovRanges.Length - 1)
            _currentCameraFOVLevel = 0;
        else _currentCameraFOVLevel++;

        float newFOV = _fovRanges[_currentCameraFOVLevel];
        ViewCam.fieldOfView = newFOV;

        HUDManager.Instance.SetZoomLevelText(_currentCameraFOVLevel + 1);
    }
}