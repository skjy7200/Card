using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public UnitData unitData;
        public EnemyAILogic aiLogic;
        public Vector3 spawnPosition;
    }

    public GameObject enemyPrefab;
    public List<EnemySpawnData> enemySpawns;
    public List<EnemyUnit> enemyUnits;

    void Start()
    {
        foreach (var spawn in enemySpawns)
        {
            var enemy = Instantiate(enemyPrefab, spawn.spawnPosition, Quaternion.identity).GetComponent<EnemyUnit>();
            enemy.Initialize(spawn.unitData, spawn.aiLogic);
            enemyUnits.Add(enemy);
        }
    }

    public List<EnemyUnit> GetSpawnedEnemies()
    {
        return enemyUnits;
    }
}

