using UnityEngine;

public abstract class FSMState : MonoBehaviour
{
    public bool StateActive { get; protected set; }

    public virtual void EnterState() => StateActive = true;
    
    
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void LateUpdateState();
    protected abstract void GetInputValues();
    
    public virtual void ExitState() => StateActive = false;
}