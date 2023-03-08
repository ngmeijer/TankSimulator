using System;
using UnityEngine;

public class TurretControlComponent : TankComponent
{
    [SerializeField] private bool _lockTurret;
    [SerializeField] private float barrelMinY;
    [SerializeField] private float barrelMaxY;
    private Vector3 currentLerpEuler;

    private void Update()
    {
        HandleTurretRotation();
        OffsetCannonRotationOnTankRotation();
    }

    private void HandleTurretRotation()
    {
        float xRotateInput = Input.GetAxis("Mouse X");

        componentManager.TurretEulerAngles += new Vector3(0, xRotateInput, 0) * (Time.deltaTime * componentManager.Properties.TurretRotateSpeed);
    }

    private void OffsetCannonRotationOnTankRotation()
    {
        float moveInput = Input.GetAxis("Vertical");
        float hullRotateInput = Input.GetAxis("Horizontal");
        float xRotateInput = Input.GetAxis("Mouse X");

        if (moveInput == 0 && hullRotateInput == 0) return;

        float offsetHullRotation = hullRotateInput * componentManager.Properties.HullRotateSpeed;
        float turretRotation = xRotateInput * componentManager.Properties.TurretRotateSpeed;
        componentManager.TurretEulerAngles += new Vector3(0, turretRotation - offsetHullRotation) * Time.deltaTime;
    }

    public float TiltCannon(float inputValue)
    {
        //Move cannon up and down
        float delta = inputValue * Time.deltaTime * componentManager.Properties.TurretTiltSpeed;
        componentManager.BarrelEulerAngles -= new Vector3(delta, 0, 0);
        componentManager.BarrelEulerAngles.x =
            Mathf.Clamp(componentManager.BarrelEulerAngles.x, barrelMaxY, barrelMinY);

        return delta;
    }
}