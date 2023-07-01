using UnityEngine;

namespace FSM
{
    public abstract class FSMState : MonoBehaviour
    {
        protected bool _stateActive;

        public virtual void Enter() => _stateActive = true;

        public virtual void UpdateState()
        {
        }

        public virtual void FixedUpdateState()
        {
        }

        public virtual void LateUpdateState()
        {
        }

        protected virtual void GetInputValues()
        {
        }

        public virtual void Exit() => _stateActive = false;
    }
}