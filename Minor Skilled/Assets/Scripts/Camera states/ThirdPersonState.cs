using UnityEditor;
using UnityEngine;

public class ThirdPersonState : CameraState
{
    [SerializeField] private float _cameraRotationDamp = 1f;
    [SerializeField] private Transform _cameraTargetDestination;
    [SerializeField] private Transform _lookAtPosition;

    [Header("Y Ranges")] [SerializeField] private Transform _cameraLowerBound;
    [SerializeField] private Transform _cameraUpperBound;

    public float RotationValue;

    private void Start()
    {
        transform.position = _cameraTargetDestination.position;
    }
    
    public override void EnterState()
    {
        base.EnterState();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void UpdateState()
    {
        UpdateThirdPersonCameraPosition();
    }

    public void UpdateThirdPersonCameraPosition()
    {
        Vector3 minY = _cameraLowerBound.position;
        Vector3 maxY = _cameraUpperBound.position;
        Vector3 maxLength = maxY - minY;

        float inverseValue = 1 - GameManager.Instance.RotationValue;
        Vector3 yDelta = inverseValue * maxLength;
        Vector3 newPosition = minY + yDelta;

        Vector3 currentPosition = ViewCam.transform.position;
        _cameraTargetDestination.position = newPosition;
        ViewCam.transform.position = Vector3.MoveTowards(currentPosition, _cameraTargetDestination.position,
            _cameraRotationDamp * Time.deltaTime);
        ViewCam.transform.LookAt(_lookAtPosition.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_lookAtPosition.position, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_cameraLowerBound.position, 0.25f);
        Handles.Label(_cameraLowerBound.position + GameManager.HandlesOffset, _cameraLowerBound.name);
        Gizmos.DrawSphere(_cameraUpperBound.position, 0.25f);
        Handles.Label(_cameraUpperBound.position + GameManager.HandlesOffset, _cameraUpperBound.name);
        Gizmos.DrawLine(_cameraLowerBound.position, _cameraUpperBound.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_cameraTargetDestination.position, 0.2f);
        Handles.Label(_cameraTargetDestination.position + GameManager.HandlesOffset,
            _cameraTargetDestination.name);

        if (ViewCam != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(ViewCam.transform.position, 0.15f);
            Handles.Label(ViewCam.transform.position + GameManager.HandlesOffset, ViewCam.name);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ViewCam.transform.position, _lookAtPosition.position);
        }
    }
}