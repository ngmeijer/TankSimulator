using System;
using DG.Tweening;
using UnityEngine;

public abstract class CameraState : FSMState
{
    public Camera ViewCam;
    public Transform StateLookAt;
    public E_CameraState ThisState;
    [SerializeField] protected float _lerpSpeed = 1f;

    public override void EnterState()
    {
        base.EnterState();
        
        SetCameraEnable(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        
        SetCameraEnable(false);
    }

    public virtual int GetFOVLevel()
    {
        return -1;
    }

    private void SetCameraEnable(bool enable)
    {
        ViewCam.gameObject.SetActive(enable);
    }
}