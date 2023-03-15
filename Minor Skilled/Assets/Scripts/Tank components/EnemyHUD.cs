using System;
using TMPro;
using UnityEngine;

public class EnemyHUD : BaseHUDManager
{
    [SerializeField] private TextMeshProUGUI _enemyName;
    private Transform _playerTransform;
    [SerializeField] private GameObject _canvas;    
    
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _canvas.transform.LookAt(_playerTransform.position);
    }

    public override void UpdateName(string name)
    {
        _enemyName.SetText(name);
    }

    public override void UpdateSpeed(float speed)
    {
        
    }
}