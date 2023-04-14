using System;
using DG.Tweening;
using UnityEngine;

public class HostileInspectorCamState : InspectorCamState
{
        public DamageRegistrationComponent _hostileComponent;
        
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
                        .OnComplete(PostLerpActions));
        }

        private void PostLerpActions()
        {
                StateLookAt.localPosition = Vector3.zero;
                _hostileComponent.ShowUI(true);
        }

        public override void ExitState()
        {
                base.ExitState();
                
                if(_hostileComponent != null)
                        _hostileComponent.ShowUI(false);
        }
}