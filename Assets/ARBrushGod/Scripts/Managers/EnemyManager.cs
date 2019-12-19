using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GoalHealth goalHealth;       // Reference to the goal's heatlh.
    public GameObject enemy;                // The enemy prefab to be spawned.
    public GameObject playZone;         //The playzone prefab
    public float spawnTime = 3f;            // How long between each spawn.
    public Transform spawnPointPrefab;         
    Transform[] spawnPoints = new Transform[16]; // An array of the spawn points this enemy can spawn from.


    void Start ()
    {
        int numOfChild = spawnPointPrefab.childCount;
        for(int i = 0; i < numOfChild; i++)
        {
            spawnPoints[i] = spawnPointPrefab.GetChild(i);
        }
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        // If the goal has no health left...
        if(goalHealth.currentHealth <= 0f)
        {
            // ... exit the function.
            return;
        }

        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        GameObject gameObject = Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        gameObject.transform.parent = playZone.transform;
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
    }
}