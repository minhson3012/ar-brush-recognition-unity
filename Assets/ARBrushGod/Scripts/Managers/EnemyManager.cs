using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GoalHealth goalHealth;       // Reference to the goal's heatlh.
    //public GameObject enemy;                // The enemy prefab to be spawned.
    public GameObject playZone;         //The playzone prefab
    public float spawnTime = 3f;            // How long between each spawn.
    public Transform spawnPointPrefab;
    public Wave[] wave;
    Transform[] spawnPoints = new Transform[8]; // An array of the spawn points this enemy can spawn from.
    int currentWave;
    int numOfWave;
    int lastIndex;

    void Start()
    {
        //Set up spawn points
        int numOfChild = spawnPointPrefab.childCount;
        for (int i = 0; i < numOfChild; i++)
        {
            spawnPoints[i] = spawnPointPrefab.GetChild(i);
        }

        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);

        //Set current wave and num of wave
        currentWave = 0;
        numOfWave = wave.Length;
    }


    void Spawn()
    {
        // If the goal has no health left...
        if (goalHealth.currentHealth <= 0f)
        {
            // ... exit the function.
            return;
        }

        //Get a random enemy from the enemy list
        int enemyToSpawnIndex = Random.Range(0, wave[currentWave].enemies.Length);
        EnemySpawn enemyToSpawn = wave[currentWave].enemies[enemyToSpawnIndex];
        GameObject enemy = enemyToSpawn.enemy;
        int enemyCount = enemyToSpawn.enemyCount;

        if (enemyCount > 0)
        {
            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            //Make sure enemies don't spawn on the same point twice
            while (spawnPointIndex == lastIndex) spawnPointIndex = Random.Range(0, spawnPoints.Length);
            lastIndex = spawnPointIndex;

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            GameObject gameObject = Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            gameObject.transform.parent = playZone.transform;
            gameObject.transform.localScale *= 0.6f;
            enemyToSpawn.enemyCount--;
        }
    }
}