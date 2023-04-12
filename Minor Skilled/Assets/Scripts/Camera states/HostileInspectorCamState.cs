using DG.Tweening;
using UnityEngine;

public class HostileInspectorCamState : InspectorCamState
{
        [SerializeField] private float _lerpSpeed = 1f;
        public Vector3 TargetPosition;

        public override void EnterState()
        { 
                base.EnterState();
                LerpToPosition();
        }
        
        private void LerpToPosition()
        { 
                _canRotateAround = false;
                StateLookAt.transform.DOMove(GameManager.Instance.HostileTargetTransform.position, _lerpSpeed)
                        .OnComplete(() => _canRotateAround = true);
        }

        public override void UpdateState()
        {
                if (!_canRotateAround) return;
                
                StateLookAt = GameManager.Instance.HostileTargetTransform;

                base.UpdateState();
        }
}