using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public abstract class CameraState : FSMState
{
    [HideInInspector] public Camera ViewCam;
    public Transform StateLookAt;
    public E_CameraState ThisState;
    [SerializeField] protected float _lerpSpeed = 0.5f;
    public bool InTransition { get; protected set; }
    public Transform CameraTargetDestination;
    public Vector3 LastCamPos;
    private bool _lookatTweenDone;
    private bool _moveToTweenDone;

    protected void Awake()
    {
        ViewCam = GetComponent<Camera>();
    }

    public override void Enter()
    {
        base.Enter();

        if (LastCamPos != Vector3.zero)
        {
            ViewCam.transform.position = LastCamPos;
            SetCameraEnable(true);
            TweenToTargetPos();
        } else SetCameraEnable(true);
        
        SetCameraEnable(true);
    }

    public override void Exit()
    {
        base.Exit();
        
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
            seq.Append(ViewCam.transform.DOLookAt(StateLookAt.position, _lerpSpeed));
        seq.OnComplete(() => InTransition = false);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(StateLookAt.position, 0.1f);
        Handles.Label(StateLookAt.position + GameManager.HandlesOffset, StateLookAt.name);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CameraTargetDestination.position, 0.1f);
        Handles.Label(CameraTargetDestination.position + GameManager.HandlesOffset, CameraTargetDestination.name);
    }
}