using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public EnemySpawn[] enemies;
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemy;
    public int enemyCount;
    public void ReduceEnemyCount()
    {
        enemyCount--;
    }
}
