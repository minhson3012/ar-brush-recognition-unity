using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    Transform goal;               // Reference to the goal's position.
    GoalHealth goalHealth;      // Reference to the goal's health.
    EnemyHealth enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.


    void Awake ()
    {
        // Set up the references.
        goal = GameObject.FindGameObjectWithTag ("Goal").transform;
        goalHealth = goal.GetComponent<GoalHealth>();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent<NavMeshAgent>();
    }


    void Update ()
    {
        // If the enemy and the goal have health left...
        if(enemyHealth.currentHealth > 0 && goalHealth.currentHealth > 0 && nav.enabled)
        {
            // ... set the destination of the nav mesh agent to the goal.
            nav.SetDestination (goal.position);
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }
    } 
}