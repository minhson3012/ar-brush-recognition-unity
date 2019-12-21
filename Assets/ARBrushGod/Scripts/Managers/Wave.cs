using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public EnemySpawn[] enemies;

    public int GetTotalEnemies()
    {
        int numOfEnemies = 0;
        foreach(EnemySpawn e in enemies)
        {
            numOfEnemies += e.enemyCount;
        }
        return numOfEnemies;
    }
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
