using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public abstract class CameraState : FSMState
{
    public Camera ViewCam;
    public Transform StateLookAt;
    public E_CameraState ThisState;
    [SerializeField] protected float _lerpSpeed = 1f;
    public bool InTransition { get; protected set; }
    public Transform CameraTargetDestination;
    private bool _lookatTweenDone;
    private bool _moveToTweenDone;

    public override void EnterState()
    {
        base.EnterState();
        
        SetCameraEnable(true);
        
        TweenToTargetPos();
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

    private void TweenToTargetPos()
    {
        InTransition = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(ViewCam.transform.DOMove(CameraTargetDestination.position, _lerpSpeed));
        if (StateLookAt != null)
            seq.Join(ViewCam.transform.DOLookAt(StateLookAt.position, _lerpSpeed));
        seq.OnComplete(() => InTransition = false);
    }

    protected virtual void OnDrawGizmos()
    {
        
    }
}