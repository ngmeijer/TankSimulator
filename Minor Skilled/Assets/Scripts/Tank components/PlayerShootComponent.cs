using UnityEngine;

public class PlayerShootComponent : BaseShootComponent
{
    private PlayerHUD _playerHUD;
    
    protected override void Start()
    {
        base.Start();
        
        _playerHUD = _componentManager.EntityHUD as PlayerHUD;
        
        _playerHUD.UpdateAmmoCountUI(_currentAmmoCount);
        _playerHUD.UpdateShellTypeUI(_currentShellType);
    }
    
    protected void Update()
    {
        //_componentManager.EntityHUD.UpdateDistanceUI(TrackDistance());
    }
    
    public override void FireShell()
    {
        base.FireShell();
        
        _playerHUD.UpdateAmmoCountUI(_currentAmmoCount);
        if (_currentAmmoCount > 0)
            StartCoroutine(_playerHUD.UpdateReloadUI(_reloadTime));
    }

    public override void SwitchShell()
    {
        _playerHUD.UpdateShellTypeUI(_currentShellType);
    }
}