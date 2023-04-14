using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonobehaviour<EnemyManager>
{
    [SerializeField] private List<Transform> _spawnedEnemies = new List<Transform>();
    public List<Transform> GetEnemies() => _spawnedEnemies;
}