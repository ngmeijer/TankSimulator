using DG.Tweening;
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

    public void CameraShake(Transform currentCamera, CameraMode camMode)
    {
        currentCamera.DOComplete();
        switch (camMode)
        {
            case CameraMode.ADS:
                currentCamera.DOShakePosition(_shakeDuration, _adsPositionShakeStrength);
                currentCamera.DOShakeRotation(_shakeDuration, _adsRotationShakeStrength);
                break;
            case CameraMode.ThirdPerson:
                currentCamera.DOShakePosition(_shakeDuration, _tpPositionShakeStrength);
                currentCamera.DOShakeRotation(_shakeDuration, _tpRotationShakeStrength);
                break;
        }
    }
}