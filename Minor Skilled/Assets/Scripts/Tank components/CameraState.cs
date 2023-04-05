using UnityEngine;

public abstract class CameraState : MonoBehaviour
{
    public Camera ViewCam;
    public Transform StateLookAt;

    public virtual void EnterState()
    {
        ViewCam.gameObject.SetActive(true);
    }

    public abstract void UpdateState();
    public abstract void LateUpdateState();
    protected abstract void GetInputValues();

    public virtual void ExitState()
    {
        ViewCam.gameObject.SetActive(false);
    }
}