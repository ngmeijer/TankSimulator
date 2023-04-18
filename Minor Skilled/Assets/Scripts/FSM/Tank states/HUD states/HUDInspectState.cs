using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDInspectState : HUDState
{
    [SerializeField] private TextMeshProUGUI _exitStateText;
    [SerializeField] private TextMeshProUGUI _rotateText;
    private InputControlScheme _controlScheme;

    public override void EnterState()
    {
        base.EnterState();

        //_controlScheme = _inputActions.controlSchemes[0];
        SetUIContent();
    }

    private void SetUIContent()
    {
        string exitKey = _inputActions.StateSwitcher.ExitState.GetBindingDisplayString();
        _exitStateText.SetText($"Press '{exitKey}' to return to the last view!");

        string enableRotationKey = _inputActions.TankInspection.AllowInspection.GetBindingDisplayString();
        _rotateText.SetText($"Hold '{enableRotationKey}' to rotate!'");
    }
    
    
    public override void ExitState()
    {
        base.ExitState();
        
    }
}