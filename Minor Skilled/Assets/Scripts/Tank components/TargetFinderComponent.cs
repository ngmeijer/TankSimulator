using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetFinderComponent : TankComponent
{
    private List<TargetIndicator> targets;
    private Camera cam;
    [SerializeField] private GameObject targetIndicatorUI;
    
    private void Start()
    {
        cam = Camera.main;
        targets = FindObjectsOfType<TargetIndicator>().ToList();
    }

    private void Update()
    {
        //Vector3 screenPos = cam.WorldToScreenPoint(targets[0].transform.position);
        //targetIndicatorUI.transform.position = new Vector3(screenPos.x, -screenPos.y, screenPos.z);
    }
}
