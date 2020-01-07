using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace BrushGestures
{
    public class EnemyMovement : MonoBehaviour
    {
        public float rotationTime = 1f;
        public float moveTime = 10f;
        public float rainMultiplier = 5f;
        public float windMultiplier = 10f;
        public bool isNearTree = false;
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
            isNearTree = false;
        }


        void Update()
        {
            if (enemyHealth.currentHealth > 0 && goalHealth.currentHealth > 0)
            {
                GameObject treeObject;
                if ((treeObject = GameObject.FindGameObjectWithTag("Tree")) != null)
                    direction = (treeObject.transform.position - transform.position).normalized;
                else
                    direction = (goal.position - transform.position).normalized;
                lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationTime);

                if (transform.position.y != goal.position.y)
                {
                    Vector3 newPosition = new Vector3(transform.position.x, goal.position.y, transform.position.z);
                    transform.position = newPosition;
                }
            }
            else SetMoveTime(100f); //Stop moving when this enemy or the goal is dead
        }

        public IEnumerator MoveToPosition(Transform transform, Vector3 position, float moveTime)
        {
            t = 0f;
            var currentPos = transform.position;
            while (t < 1 && !enemyAttack.goalInRange)
            {
                yield return new WaitUntil(() => !isNearTree); // Don't move until tree is gone
                t += Time.deltaTime / currentMoveTime;
                transform.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }
        }

        public void SetMoveTime(float duration, string slowType = "default")
        {
            if (!isSlowed)
            {
                if (slowType == "rain")
                {
                    currentMoveTime = currentMoveTime * rainMultiplier; //Different rain slow multiplier for different zombies
                }
                else if (slowType == "wind") currentMoveTime = currentMoveTime * rainMultiplier;
                else if (slowType == "default") currentMoveTime *= 100f;
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
}