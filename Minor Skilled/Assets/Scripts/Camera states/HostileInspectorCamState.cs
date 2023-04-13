using System;
using DG.Tweening;
using UnityEngine;

public class HostileInspectorCamState : InspectorCamState
{
        public override void EnterState()
        {
                base.EnterState();
                
                LerpToPosition();
        }
        
        private void LerpToPosition()
        {
                StateLookAt.parent = GameManager.Instance.HostileTargetTransform;
                Sequence lerpSeq = DOTween.Sequence();
                lerpSeq.Append(ViewCam.transform.DOLookAt(GameManager.Instance.HostileTargetTransform.position, _lerpSpeed));
                lerpSeq.Append(StateLookAt.DOMove(GameManager.Instance.HostileTargetTransform.position, _lerpSpeed)
                        .OnUpdate(() => ViewCam.transform.LookAt(GameManager.Instance.HostileTargetTransform.position))
                        .OnComplete(() => StateLookAt.localPosition = Vector3.zero));
        }
}