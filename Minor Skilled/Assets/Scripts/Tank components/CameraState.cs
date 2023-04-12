using UnityEngine;

public abstract class CameraState : FSMState
{
    public Camera ViewCam;
    public Transform StateLookAt;
    public E_CameraState ThisState;

    public override void EnterState()
    {
        base.EnterState();
        
        ViewCam.gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        
        ViewCam.gameObject.SetActive(false);
    }

    public virtual int GetFOVLevel()
    {
        return -1;
    }
}