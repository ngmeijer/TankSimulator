using System;
using DG.Tweening;
using UnityEngine;

public abstract class CameraState : FSMState
{
    public Camera ViewCam;
    public Transform StateLookAt;
    public E_CameraState ThisState;
    protected Vector3 _defaultCamPos;
    public Vector3 PreviousCameraPos;
    public Transform NextCameraTrans;
    [SerializeField] protected float _lerpSpeed = 1f;


    private void Start()
    {
        _defaultCamPos = ViewCam.transform.position;
    }

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

    protected void SetCameraEnable(bool enable)
    {
        ViewCam.gameObject.SetActive(enable);
    }
}