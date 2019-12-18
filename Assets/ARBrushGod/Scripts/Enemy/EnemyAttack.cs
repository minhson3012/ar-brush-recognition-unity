using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage;

    Animator animator;
    GameObject goal;
    GoalHealth goalHealth;
    EnemyHealth enemyHealth;
    bool goalInRange;
    float timer;

    // Start is called before the first frame update
    void Awake()
    {
        goal = GameObject.FindGameObjectWithTag("Goal");
        goalHealth = goal.GetComponent<GoalHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == goal)
        {
            animator.SetTrigger("Attack");
            goalInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == goal)
        {
            animator.SetTrigger("StopAttack");
            goalInRange = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && goalInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
        }

        if(goalHealth.currentHealth <= 0)
        {
            animator.SetTrigger("GoalDestroyed");
        }
    }

    void Attack()
    {
        timer = 0f;
        
        if(goalHealth.currentHealth > 0)
        {
            goalHealth.TakeDamage(attackDamage);
        }
    }
}
