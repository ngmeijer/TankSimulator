public class AdsView : CameraView
{
    private float[] _fovRanges = {
        60f,
        20f,
        10f,
        5f,
        2.5f
    };
    
    private int _currentCameraFOVLevel = 0;

    private void Start()
    {
        HUDManager.Instance.SetZoomLevelText(_currentCameraFOVLevel + 1);
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