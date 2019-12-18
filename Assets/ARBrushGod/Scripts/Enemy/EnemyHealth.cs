using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth;
    public float currentHealth;
    public float moveSpeed;
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
        healthCanvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
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
    }

    public void StartSinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2f);
    }
}
