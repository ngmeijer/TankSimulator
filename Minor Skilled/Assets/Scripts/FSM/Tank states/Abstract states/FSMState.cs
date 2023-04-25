using UnityEngine;

public abstract class FSMState : MonoBehaviour
{
    //2 possible approaches:
    //1.
    //      _stateActive that gets set on Enter/ExitState
    //      No virtual/abstract Updates, but needs to check in every Monobehaviour loop if(!stateActive) return
    
    //2.
    //      No _stateActiveBools, but virtual/abstract Updates are declared.
    //      currentState.Update/LateUpdate/FixedUpdate must be called in StateSwitcher, but no need to check if(!stateActive) since only the active state is updated
    
    protected bool _stateActive;

    public virtual void Enter() => _stateActive = true;

    public virtual void UpdateState() {}
    public virtual void FixedUpdateState() {}
    public virtual void LateUpdateState() {}
    protected virtual void GetInputValues() {}

    public virtual void Exit() => _stateActive = false;
}