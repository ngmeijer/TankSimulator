using DG.Tweening;
using Tank_components;
using TankComponents;
using UnityEngine;

public class CameraShakeComponent : TankComponent
{
    [SerializeField] private float _shakeDuration;

    [Header("ADS")] 
    [SerializeField] private Vector3 _adsPositionShakeStrength;
    [SerializeField] private Vector3 _adsRotationShakeStrength;
    
    [Header("ADS")] 
    [SerializeField] private Vector3 _tpPositionShakeStrength;
    [SerializeField] private Vector3 _tpRotationShakeStrength;

    public void CameraShake(Transform currentCamera, E_CameraState camState)
    {
        currentCamera.DOComplete();
        switch (camState)
        {
            case E_CameraState.ADS:
                currentCamera.DOShakePosition(_shakeDuration, _adsPositionShakeStrength);
                currentCamera.DOShakeRotation(_shakeDuration, _adsRotationShakeStrength);
                break;
            case E_CameraState.ThirdPerson:
                currentCamera.DOShakePosition(_shakeDuration, _tpPositionShakeStrength);
                currentCamera.DOShakeRotation(_shakeDuration, _tpRotationShakeStrength);
                break;
        }
    }
}