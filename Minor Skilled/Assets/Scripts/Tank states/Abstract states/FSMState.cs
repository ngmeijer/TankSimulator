using UnityEngine;

public abstract class FSMState : MonoBehaviour
{
    public bool StateActive { get; protected set; }

    public virtual void EnterState() => StateActive = true;
    
    
    public virtual void UpdateState() {}
    public virtual void FixedUpdateState() {}
    public virtual void LateUpdateState() {}
    protected virtual void GetInputValues() {}
    
    public virtual void ExitState() => StateActive = false;
}