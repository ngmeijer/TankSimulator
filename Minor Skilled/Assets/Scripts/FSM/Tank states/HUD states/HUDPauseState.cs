using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDPauseState : HUDState
{
    [SerializeField] private Ease _selectedEase;
    [SerializeField] private float _targetXPos;
    [SerializeField] private float _tweenSpeed;
    private Vector3 _pauseMenuStartPos;
    
    public override void Enter()
    {
        base.Enter();
        ShowUI();
    }

    public void ShowUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _pauseMenuStartPos = HUDContainer.transform.position;
        transform.DOMoveX(_targetXPos, _tweenSpeed)
            .SetEase(_selectedEase);
        Cursor.visible = true;
    }

    public void HideUI()
    {
        HUDContainer.transform.DOMoveX(_pauseMenuStartPos.x, _tweenSpeed)
            .SetEase(_selectedEase)
            .OnComplete(() => base.Exit());
        Cursor.visible = false;
    }

    public override void Exit()
    {
        HideUI();
    }
}