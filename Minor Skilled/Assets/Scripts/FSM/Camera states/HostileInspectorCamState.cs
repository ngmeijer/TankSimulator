using System;
using DG.Tweening;
using UnityEngine;

public class HostileInspectorCamState : InspectorCamState
{
        public DamageRegistrationComponent _hostileComponent;

        public override void Enter()
        {
                base.Enter();

                GameManager.Instance.HostileManager.TryGetComponent(out TankComponentManager tankManager);
                _hostileComponent = tankManager.DamageComp;

                LerpToPosition(_hostileComponent.transform);
        }
        
        private void LerpToPosition(Transform to)
        {
                InTransition = true;
                StateLookAt.parent = to;
                Sequence lerpSeq = DOTween.Sequence();
                lerpSeq.Append(ViewCam.transform.DOLookAt(to.position, _lerpSpeed));
                lerpSeq.Append(StateLookAt.DOMove(to.position, _lerpSpeed)
                        .OnUpdate(() => ViewCam.transform.LookAt(to.position))
                        .OnComplete(PostLerpActions));
        }

        private void PostLerpActions()
        {
                InTransition = false;
                StateLookAt.localPosition = Vector3.zero;
                _hostileComponent.ShowUI(true);
        }

        public override void Exit()
        {
                base.Exit();
                
                if(_hostileComponent != null)
                        _hostileComponent.ShowUI(false);
        }
}