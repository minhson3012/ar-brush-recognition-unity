using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth;
    public float currentHealth;
    public int attackDamage;
    public Image healthBar;
    public float sinkSpeed = 0.1f;
    bool isDead = false;
    bool isSinking = false;
    Animator animator;
    Canvas healthCanvas;
    void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        healthCanvas = GetComponentInChildren<Canvas>();
        healthCanvas.worldCamera = Camera.main.GetComponent<Camera>();
    }

    void Update()
    {
        healthCanvas.transform.LookAt(Camera.main.transform);
        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / startingHealth;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        animator.SetTrigger("Dead");

        //Tell EnemyManager that an enemy is dead
        GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().EnemyDead();
    }

    public void StartSinking()
    {
        // GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2f);
    }
}
