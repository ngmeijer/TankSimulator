using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] private Volume _postProcessing;

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
            SetPostProcessingEnable(true);
            SetCameraEnable(true);
            TweenToTargetPos();
        } else SetCameraEnable(true);
        
        SetCameraEnable(true);
    }

    public override void Exit()
    {
        base.Exit();
        
        SetPostProcessingEnable(false);
        SetCameraEnable(false);
    }

    public virtual int GetFOVLevel()
    {
        return -1;
    }

    public void SetCameraPosition(Vector3 position)
    {
        ViewCam.transform.position = position;
    }

    private void SetCameraEnable(bool enable)
    {
        ViewCam.gameObject.SetActive(enable);
    }
    
    private void SetPostProcessingEnable(bool enabled)
    {
        if (_postProcessing == null)
        {
            Debug.Log($"PostProcessing for {this.name} is null.");
            return;
        }

        _postProcessing.enabled = enabled;
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