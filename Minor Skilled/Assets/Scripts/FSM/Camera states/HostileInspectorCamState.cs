using System;
using DG.Tweening;
using UnityEngine;

public class HostileInspectorCamState : InspectorCamState
{
        public DamageRegistrationComponent _hostileComponent;

        public override void EnterState()
        {
                base.EnterState();

                GameManager.Instance.HostileTargetTransform.TryGetComponent(out TankComponentManager tankManager);
                _hostileComponent = tankManager.DamageComponent;

                LerpToPosition(GameManager.Instance.HostileTargetTransform);
        }
        
        private void LerpToPosition(Transform to)
        {
                _inAnimation = true;
                StateLookAt.parent = to;
                Sequence lerpSeq = DOTween.Sequence();
                lerpSeq.Append(ViewCam.transform.DOLookAt(to.position, _lerpSpeed));
                lerpSeq.Append(StateLookAt.DOMove(to.position, _lerpSpeed)
                        .OnUpdate(() => ViewCam.transform.LookAt(to.position))
                        .OnComplete(PostLerpActions));
        }

        private void PostLerpActions()
        {
                _inAnimation = false;
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