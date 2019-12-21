using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class EnemyMovement : MonoBehaviour
{
    public float rotationTime = 1f;
    public float moveTime = 10f;
    Transform goal;               // Reference to the goal's position.
    GoalHealth goalHealth;      // Reference to the goal's health.
    EnemyHealth enemyHealth;        // Reference to this enemy's health.
    EnemyAttack enemyAttack;
    // NavMeshAgent nav;               // Reference to the nav mesh agent.

    Quaternion lookRotation;
    Vector3 direction;
    float currentMoveTime;
    float t = 0f;
    bool isSlowed;

    void Start()
    {
        // Set up the references.
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        goalHealth = goal.GetComponent<GoalHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        currentMoveTime = moveTime;
        StartCoroutine(MoveToPosition(transform, goal.position, currentMoveTime));
        isSlowed = false;
    }


    void Update()
    {
        if (enemyHealth.currentHealth > 0 && goalHealth.currentHealth > 0)
        {
            direction = (goal.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationTime);

            if(transform.position.y != goal.position.y)
            {
                Vector3 newPosition = new Vector3(transform.position.x, goal.position.y, transform.position.z);
                transform.position = newPosition;
            }
        }
        else SetMoveTime(100f, 2f);
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float moveTime)
    {
        var currentPos = transform.position;
        t = 0f;
        while (t < 1 && !enemyAttack.goalInRange)
        {
            t += Time.deltaTime / currentMoveTime;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }

    public void SetMoveTime(float multiplier, float duration)
    {
        if (!isSlowed)
        {
            currentMoveTime = currentMoveTime * multiplier;
            isSlowed = true;
            StartCoroutine(ResetMoveTime(duration));
        }
    }

    IEnumerator ResetMoveTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentMoveTime = moveTime;
        isSlowed = false;
    }
}